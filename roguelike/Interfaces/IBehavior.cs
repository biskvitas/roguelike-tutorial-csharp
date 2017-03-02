using roguelike.Core.Systems;
using roguelike.Entities.Monsters;

namespace roguelike.Interfaces
{
    public interface IBehavior
	{
		bool Act(Monster monster, CombatSystem combatSystem);
	}
}
