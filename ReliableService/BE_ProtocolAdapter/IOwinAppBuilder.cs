namespace ProtocolAdapter
{
    namespace WebApi
    {
        using Owin;

        public interface IOwinAppBuilder
        {
            void Configuration(IAppBuilder appBuilder);
        }
    }
}