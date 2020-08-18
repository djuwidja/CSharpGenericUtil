using Djuwidja.GenericUtil.Patterns.IoC;
using System;
using System.Collections.Generic;
using System.Text;

namespace Djuwidja.GenericUtil.Patterns.IoC.Example
{
    public class Example
    {
        public void StaticInstantiation()
        {
            DjaOC.Instantiate();
            DjaOC.BindNewInstance<Weapon, HeroDefaultWeapon>();
            DjaOC.BindNewInstance<Armor, HeroDefaultArmor>();
            DjaOC.BindNewInstance<Character, Hero>();

            Hero hero = (Hero) DjaOC.Get<Character>("hero");
            Console.WriteLine("This hero has weapon with atk = {0} and def = {1}", hero.Weapon.Atk, hero.Armor.Def);

            Weapon excalibur = new Weapon(8);
            DjaOC.Bind<Weapon>(excalibur, "excalibur");

            Armor genjiArmor = new Armor(8);
            DjaOC.Bind<Armor>(genjiArmor, "genjiArmor");
        }

        public void InstancedInstantiation()
        {
            Injector injector = new Injector();
            injector.BindNewInstance<Weapon, HeroDefaultWeapon>();
            injector.BindNewInstance<Armor, HeroDefaultArmor>();
            injector.BindNewInstance<Character, Hero>();

            Hero hero = (Hero) injector.Get<Character>("hero");
            Console.WriteLine("This hero has weapon with atk = {0} and def = {1}", hero.Weapon.Atk, hero.Armor.Def);

            Weapon excalibur = new Weapon(8);
            injector.Bind<Weapon>(excalibur, "excalibur");

            Armor genjiArmor = new Armor(8);
            injector.Bind<Armor>(genjiArmor, "genjiArmor");
        }
    }
}
