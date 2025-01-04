from flask import Flask, request, jsonify
from tensorflow.keras.preprocessing import image
from tensorflow.keras.applications.resnet50 import preprocess_input
import numpy as np
import keras
import pyodbc
import pandas as pd
from sklearn.metrics.pairwise import cosine_similarity
from DeepImageSearch import Load_Data, Search_Setup
import better_profanity

app = Flask(__name__)

# Load your ResNet50 model
resnet_model = keras.models.load_model('D:\\ASP.NET\\CreativeCollabOla\\CreativeCollab2\\CreativeCollab2\\my_model.keras')

# Path to the parent folder containing subfolders with images
parent_folder_path = r'D:\ASP.NET\CreativeCollabOla\CreativeCollab2\CreativeCollab2\wwwroot\images\post'

# Load images from subfolders within the parent folder
image_list = Load_Data().from_folder([parent_folder_path])

num_images = len(image_list)

# Set up the search engine, specifying the model and other parameters
st = Search_Setup(image_list=image_list, model_name='resnet50', pretrained=True, image_count=num_images)

# Index the images
st.run_index()

# Define endpoint to receive image path and return predictions
@app.route('/predict', methods=['POST'])
def predict():
    image_path = request.json['image_path']
    predicted_classes = predict2(image_path)
    return jsonify({'predictions': predicted_classes})

def predict2(img_path):
    img = image.load_img(img_path, target_size=(224, 224))
    img_array = image.img_to_array(img)
    img_array = np.expand_dims(img_array, axis=0)
    img_array = preprocess_input(img_array)
    class_probs = resnet_model.predict(img_array)[0]
    class_indices = {
        'Architecture': 0,
        'Art': 1,
        'Calligraphy': 2,
        'Cooking': 3,
        'Fashion': 4,
        'Photography': 5,
        'Plants': 6,
        'Sculpture': 7
    }
    indices_to_classes = {v: k for k, v in class_indices.items()}
    threshold = 0.02
    predicted_classes_indices = [i for i, prob in enumerate(class_probs) if prob > threshold]
    predicted_classes = [indices_to_classes[i] for i in predicted_classes_indices]
    return predicted_classes




import pyodbc
from flask import Flask, request, jsonify
import pandas as pd
import numpy as np
import random
# Connection parameters
# Connection parameters
server = 'FARAHYASSER'
database = 'CreativeCollab'


# Construct connection string
conn_str = (
    f'DRIVER={{SQL Server}};'
    f'SERVER={server};'
    f'DATABASE={database};'
    'Trusted_Connection=yes;'
)

# Connect to the database
conn = pyodbc.connect(conn_str)

# SQL query
sql_query = """
SELECT
    l.UserID,
    l.PostID,
    COALESCE((
        SELECT STRING_AGG(int.InterestName, '|')
        FROM Images i
        JOIN Interests int ON i.InterestID = int.InterestID
        WHERE i.ImageName IN (
            SELECT i2.ImageName
            FROM Images i2
            WHERE i2.ImageID = p.PostImageID
        )
        GROUP BY i.ImageName
    ), '') AS InterestNames
FROM
    Likes l
JOIN
    Posts p ON l.PostID = p.PostID;
"""

# Execute the query and fetch data into a DataFrame
df = pd.read_sql_query(sql_query, conn)

# SQL query
sql_query2 = """
SELECT [PostID], [UserID]
FROM [CreativeCollab].[dbo].[Posts];
"""

# Execute the query and fetch data into a DataFrame
df_posts = pd.read_sql_query(sql_query2, conn)


# specific_user_id='222708a5-ca9c-43e0-81cc-4245f4544ba2'

# picked_userid='52c60cf3-e067-44f9-b2ae-852feebc4d48'
# specific_user_id='52c60cf3-e067-44f9-b2ae-852feebc4d48'
# picked_userid='52c60cf3-e067-44f9-b2ae-852feebc4d48'
# picked_userid='050cbcc1-0aae-4cd0-bac9-e338a599ff30'
# specific_user_id='050cbcc1-0aae-4cd0-bac9-e338a599ff30'
# fav_post_id=423
# fav_post_id=chosen_post_id
# Assuming you have the functions defined as before
@app.route('/receive-data', methods=['POST'])
def receive_data():
    data = request.get_json()
    specific_user_id = data.get('UserID')
    # specific_liked_post = data.get('UserLikes')
    user_interests_df = df[(df['UserID'] == specific_user_id) & df['InterestNames'].isin(df['InterestNames'].unique())]
    chosen_posts_by_interest = {}
    for index, row in user_interests_df.iterrows():
        interest = row['InterestNames']
        post_id = row['PostID']
        if interest not in chosen_posts_by_interest:
            chosen_posts_by_interest[interest] = post_id
    # fav_post_id=chosen_posts_by_interest            
    recommendations = perform_recommendation(df, specific_user_id, chosen_posts_by_interest)
    return jsonify((recommendations))

def perform_recommendation(df, picked_userid, fav_post_id):
    
    def get_collaborative_recommendations(picked_userid, N=20):
        df['Liked'] = 1
        matrix = df.pivot_table(index='UserID', columns='PostID', values='Liked', fill_value=np.nan)
        
        matrix_norm = matrix.fillna(0)
        user_similarity = matrix_norm.T.corr()
        user_similarity.drop(index=picked_userid, inplace=True)
        user_similarity_threshold = 0.4
        similar_users = user_similarity[user_similarity[picked_userid] > user_similarity_threshold][picked_userid].sort_values(ascending=False)
        picked_userid_seen = matrix_norm.loc[matrix_norm.index == picked_userid].dropna(axis=1, how='all')
        similar_user_posts = matrix_norm[matrix_norm.index.isin(similar_users.index)].dropna(axis=1, how='all')
        picked_userid_seen = picked_userid_seen.loc[:, (picked_userid_seen != 0).any(axis=0)]
        similar_user_posts = similar_user_posts.loc[:, (similar_user_posts != 0).any(axis=0)]
        item_score = {}
        for i in similar_user_posts.columns:
            post_rating = similar_user_posts[i]
            total = 0
            count = 0
            for u in similar_users.index:
                if pd.isna(post_rating[u]) == False:
                    score = similar_users[u] * post_rating[u]
                    total += score
                    count += 1
            item_score[i] = total / count        
        item_score = pd.DataFrame(item_score.items(), columns=['post', 'post_score'])
        ranked_item_score = item_score.sort_values(by='post_score', ascending=False)
        user_liked_posts = df[df['UserID'] == picked_userid]['PostID'].tolist()
        ranked_item_score = ranked_item_score[~ranked_item_score['post'].isin(user_liked_posts)]

        # m = 20
        ranked_item_score = ranked_item_score[~ranked_item_score['post'].isin(user_liked_posts)]        
        return ranked_item_score[:N]

    def get_content_based_recommendations(picked_userid, fav_post_id, n_recommendations=20):
        fav_post_ids = list(fav_post_id.values())
        user_liked_posts = df[df['UserID'] == picked_userid]['PostID'].tolist()
        user_liked_posts_filtered = [post_id for post_id in user_liked_posts if post_id not in fav_post_ids]
        post_features_filtered = df[~df['PostID'].isin(user_liked_posts_filtered)][['PostID', 'InterestNames']].drop_duplicates()
        post_features_filtered['InterestNames'] = post_features_filtered['InterestNames'].str.split('|')
        unique_categories = set(category for sublist in post_features_filtered['InterestNames'] for category in sublist)                
        for category in unique_categories:
            post_features_filtered[category] = post_features_filtered['InterestNames'].apply(lambda x: int(category in x))        
        post_features_filtered = post_features_filtered.set_index('PostID')
        post_features_filtered.drop(columns=['InterestNames'], inplace=True)        
        from sklearn.metrics.pairwise import cosine_similarity
        post_similarity = cosine_similarity(post_features_filtered.values)    
        similar_posts = []
        # Iterate over each favorite post
        for fav_post_id in fav_post_ids:
            target_post_index = post_features_filtered.index.get_loc(fav_post_id)
            target_post_similarities = post_similarity[target_post_index]
            similar_posts_indices = np.where(target_post_similarities > 0.8)[0]  # Adjust the threshold as needed
            similar_posts.extend(post_features_filtered.iloc[similar_posts_indices].index.tolist())
        # Remove duplicates and posts already liked by the user
        similar_posts = list(set(similar_posts))
        similar_posts = [post_id for post_id in similar_posts if post_id not in user_liked_posts]        
        return similar_posts[:n_recommendations]


    def hybrid_recommendations(picked_userid, fav_post_id, n_recommendations=20, collaborative_weight=0.5):
        collaborative_recs_df = get_collaborative_recommendations(picked_userid, N=5)
        collaborative_recs = set(collaborative_recs_df['post'].tolist())        
        content_based_recs = get_content_based_recommendations(picked_userid, fav_post_id, n_recommendations)
        posts_by_user = df_posts[df_posts['UserID'] == picked_userid]['PostID'].tolist()
        filtered_hybrid_recs = [post_id for post_id in collaborative_recs.union(content_based_recs) if post_id not in posts_by_user]
        hybrid_recs = {}
        for post_id in filtered_hybrid_recs:
            score_collab = collaborative_recs_df.loc[collaborative_recs_df['post'] == post_id, 'post_score'].values[0] if post_id in collaborative_recs else 0
            score_content = n_recommendations - content_based_recs.index(post_id) if post_id in content_based_recs else 0  # Inverse rank as score
            hybrid_score = collaborative_weight * score_collab + (1 - collaborative_weight) * score_content
            hybrid_recs[post_id] = hybrid_score
        sorted_hybrid_recs = sorted(hybrid_recs.items(), key=lambda x: x[1], reverse=True)
        top_hybrid_recs = sorted_hybrid_recs[:n_recommendations]
        post_ids = [post_id for post_id, _ in top_hybrid_recs]        
        
        return post_ids


    final_recommendations = hybrid_recommendations(picked_userid, fav_post_id, n_recommendations=20, collaborative_weight=0.5)

    return final_recommendations
# imagePath = r'D:\ASP.NET\CreativeCollabOla\CreativeCollab2\CreativeCollab2\wwwroot\images\search\fe2617fd-f05d-4fc6-94f9-c281d3969b67.png'
@app.route('/search-similar', methods=['POST'])
def search_similar():
    image_path = request.json['image_path2']
    print(image_path)
    with open(image_path, 'rb') as f:
        print(f)
        similar_images = search_similar_images(f)
    print(similar_images)
    return jsonify({'similar_images': similar_images})

def search_similar_images(image_file, number_of_images=10):
    similar_images_dict = st.get_similar_images(image_path=image_file, number_of_images=number_of_images)
    similar_images = list(similar_images_dict.values())
    return similar_images

@app.route('/check-profanity', methods=['POST'])
def check_profanity():
    data = request.get_json()
    sentence = data.get('sentence')
    if sentence:
        contains_profanity = better_profanity.profanity.contains_profanity(sentence)
        print(contains_profanity)
        return jsonify(contains_profanity)
   
    
if __name__ == '__main__':
    app.run(debug=False)

# Close the connection
conn.close()