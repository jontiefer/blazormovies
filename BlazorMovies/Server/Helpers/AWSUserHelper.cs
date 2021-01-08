using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMovies.Server.Helpers
{
    public static class AWSUserHelper
    {        
        public static bool EnvVarCredentialsExist()
        {
            try
            {
                EnvironmentVariablesAWSCredentials envVarsAWSCredentials = new EnvironmentVariablesAWSCredentials();
                var credentials = envVarsAWSCredentials.FetchCredentials();

                return credentials != null;
            }
            catch(Exception)
            {
                return false;
            }
        }       
        

        public static void ConfigureAWSOptions(AWSOptions options)
        {
            if (EnvVarCredentialsExist())
            {
                EnvironmentVariablesAWSCredentials envVarsAWSCredentials = new EnvironmentVariablesAWSCredentials();
                var credentials = envVarsAWSCredentials.FetchCredentials();

                EnvironmentVariableAWSRegion envVarsAWSregion = new EnvironmentVariableAWSRegion();

                options.Credentials = new BasicAWSCredentials(credentials.AccessKey, credentials.SecretKey);
                options.Region = envVarsAWSregion.Region;

                Console.WriteLine("LOG: AWS Environment Variable Credentials found and will be set.");
            }   
            // Let Credential configuration resolve automatically to the lowest order of the AWS credential fallback
            // model.  In this case, if process-level environment variables are not found on the system,
            // then it will resort to acquiring IAM Task credentials through the Instance Metadata Storage
            // of the service.
            else
            {
                // Add additional custom credential source aquisition logic here.

                //Will return URI of container for logging purposes when ECS Task credentials will be used 
                //and loeded from the Instance Metadata service (IMDS) to apply the IAM role of the task to the services being used in the application.
                string uriContainerCred = Environment.GetEnvironmentVariable(ECSTaskCredentials.ContainerCredentialsURIEnvVariable);

                if(string.IsNullOrEmpty(uriContainerCred))
                    uriContainerCred = Environment.GetEnvironmentVariable(ECSTaskCredentials.ContainerCredentialsFullURIEnvVariable);

                if (!string.IsNullOrEmpty(uriContainerCred))
                    Console.WriteLine($"LOG: IMDS to be used to supply credentials for ECS Task.  Container Credentials URI: {uriContainerCred}");
            }            
        }
            
        public static AmazonS3Client GetS3Client()
        {
            EnvironmentVariablesAWSCredentials envVarsAWSCredentials = new EnvironmentVariablesAWSCredentials();
            var credentials = envVarsAWSCredentials.FetchCredentials();

            EnvironmentVariableAWSRegion envVarAWSRegion = new EnvironmentVariableAWSRegion();

            return new AmazonS3Client(credentials.AccessKey, credentials.SecretKey, envVarAWSRegion.Region);
        }
    }
}
