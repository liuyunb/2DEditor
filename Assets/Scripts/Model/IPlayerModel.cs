using FrameWorkDesign;

namespace Model
{
    public interface IPlayerModel : IModel
    {
        BindableProperty<int> HP { get; }
    }
    
    public class PlayerModel : AbstractModel, IPlayerModel
    {
        protected override void OnInit()
        {
            
        }

        public BindableProperty<int> HP { get; } = new BindableProperty<int>()
        {
            Value = 3
        };
    }
}