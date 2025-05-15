namespace http_request_monitoring_system
{
    public class Program
    {
        public static HttpClient client = new HttpClient();
        public static HttpServer server = new HttpServer();
        public static ServerActions serverActions = new ServerActions();

        public static void Main(string[] args)
        {
            Dictionary<string, Route> getRouter = new Dictionary<string, Route>();
            getRouter.Add("/", serverActions.GetServerInfo);

            server.Router.Add("GET", getRouter);

            Dictionary<string, Route> postRouter = new Dictionary<string, Route>();
            postRouter.Add("/getData", serverActions.GetData);
            postRouter.Add("/addData", serverActions.AddData);

            server.Router.Add("POST", postRouter);

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                      policy =>
                      {
                          policy.WithOrigins("http://127.0.0.1:5500", "http://localhost:5500", "https://localhost:5500", "https://127.0.0.1:5500")
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials();
                      });
            });

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            /*
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            */

            // app.UseHttpsRedirection();

            app.UseCors();

            // app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
