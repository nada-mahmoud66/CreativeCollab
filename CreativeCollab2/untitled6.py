import better_profanity

def contains_bad_word(sentence):
    return better_profanity.profanity.contains_profanity(sentence)

sentence = input("Enter a sentence: ")
print(contains_bad_word(sentence))