using Serilog;

namespace LearnService
{
    public class SampleService
    {
        private readonly string _serviceName;

        public SampleService(string serviceName)
        {
            _serviceName = serviceName;
        }

        public void Start()
        {
            Log.Debug("Starting Service {serviceName}", _serviceName);
        }

        public void Stop()
        {
            Log.Debug("Stopping Service {serviceName}", _serviceName);
        }
    }
}