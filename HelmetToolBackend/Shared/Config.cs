namespace HelmetToolBackend.Shared
{
    public class Config
    {
        public string CosmosDbConnectionString = Environment.GetEnvironmentVariable("CosmosDbConnectionString")
            ?? throw new Exception("CosmosDbConnectionString is not set.");
        public string CosmosDbDatabaseName = Environment.GetEnvironmentVariable("DatabaseName")
            ?? throw new Exception("DatabaseName is not set.");

        public string SecretKey = Environment.GetEnvironmentVariable("SecretKey")
            ?? throw new Exception("SecretKey is not set.");
        public string CosmosDbContainerNameLibrary = "library";
        public string CosmosDbContainerNameChallengeAnswers = "challenge-answers";
        public string CosmosDbContainerNameChallenge = "challenges";
        public string CosmosDbContainerNameAnswers = "answers";
        public string CosmosDbContainerNameUsers = "users";
        public string CosmosDbContainerNameItems = "items";
    }
}