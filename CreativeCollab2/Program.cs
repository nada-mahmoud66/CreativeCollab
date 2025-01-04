using CreativeCollab2.BL;
using CreativeCollab2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static CreativeCollab2.BL.ClsNotifications;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR(); // Add SignalR service

builder.Services.AddDbContext<CreativeCollabContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<Iposts, ClsUserPosts>();
builder.Services.AddScoped<Iintersts, ClsIntersts>();
builder.Services.AddScoped<Ilikes, ClsLikes>();
builder.Services.AddScoped<Icomments, ClsComments>();

builder.Services.AddScoped<Ifollow, ClsFollow>();

builder.Services.AddScoped<Igroups, ClsGroups>();
builder.Services.AddScoped<Irows, ClsRows>();
builder.Services.AddScoped<IGroups, Groups>();

builder.Services.AddScoped<IChats, ClsChats>();


builder.Services.AddScoped<INotifications, ClsNotifications>();
builder.Services.AddScoped<Iserch, ClsSearch>();



builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
	options.Password.RequiredLength = 8;
	options.Password.RequireNonAlphanumeric = true;
	options.Password.RequireUppercase = true;
	options.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<CreativeCollabContext>();

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();

builder.Services.Configure<IISServerOptions>(options =>
{
	options.AllowSynchronousIO = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();
app.UseSession();
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=User}/{action=Login}/{id?}");

app.MapHub<ChatHub>("/chatHub"); // Map the SignalR hub

app.MapHub<NotificationHub>("/notificationHub");
app.MapHub<TimelineHub>("/timelineHub");


app.Run();
