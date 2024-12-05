var builder = WebApplication.CreateBuilder(args); //tis builder is kestrel web server
var app = builder.Build(); // tis app  is te web application

app.MapGet("/", () => "Hello World! welcome to minimal api");  //tis is te middlwear component
//containin a routin and lamda expression (a call bark functiona function tat invoke itself)

app.Run(); //tis run web application
