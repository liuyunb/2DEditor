namespace FrameWorkDesign.Rule
{
    public interface ICanSendQuery : IBelongToArchitecture
    {
        
    }

    public static class CanSendQueryExtension
    {
        public static T SendQuery<T>(this ICanSendQuery self, IQuery<T> query)
        {
            return self.GetArchitecture().SendQuery(query);
        }
    }
}