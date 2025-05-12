namespace http_request_monitoring_system
{
    public class Program
    {
        public static HttpClient client = new HttpClient();
        public static HttpServer server = new HttpServer();
        public static ServerActions serverActions = new ServerActions();

        public static void Main(string[] args)
        {
            /*
            server.GetRouter.Add("/", serverActions.GetServerInfo);
            server.PostRouter.Add("/getData", serverActions.GetData);
            server.PostRouter.Add("/addData", serverActions.AddData);
            */

            Dictionary<string, Route> getRouter = new Dictionary<string, Route>();
            getRouter.Add("/", serverActions.GetServerInfo);

            server.Router.Add("GET", getRouter);

            Dictionary<string, Route> postRouter = new Dictionary<string, Route>();
            postRouter.Add("/getData", serverActions.GetData);
            postRouter.Add("/addData", serverActions.AddData);

            server.Router.Add("POST", postRouter);

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
