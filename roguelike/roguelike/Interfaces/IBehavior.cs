using roguelike.Core;
using roguelike.Systems;

namespace roguelike.Interfaces
{
	public interface IBehavior
	{
		bool Act(Monster monster, CommandSystem commandSystem);
	}
}
