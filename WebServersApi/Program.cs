

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost",
                                              "http://localhost:5173",
                                              "http://localhost:3000")
                                            .AllowAnyHeader()
                                            .AllowAnyMethod()
                                            .WithMethods("PUT", "DELETE", "GET", "POST");
                      });

    /*options.AddPolicy("WelcomePolicy",
                      policy =>
                      {
                          policy.WithOrigins("http://localhost",
                                              "http://localhost:5173",
                                              "http://localhost/3000")
                                            .AllowAnyHeader()
                                            .AllowAnyMethod()
                                            .WithMethods("PUT", "DELETE", "GET", "POST");
                      });*/
});

builder.Services.AddControllers();

//builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

// Add services to the container.
//builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

//app.MapControllerRoute(
//    name: "Products",
//    pattern: "{controller=Products}/{action=Index}/{id?}");

app.MapControllers();

app.Run();