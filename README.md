# Warbase
BaseWars for S&amp;box, reimagined

## Introduction

We want to preserve these factors of BaseWars:



* An idle-game-like progression system
* High tension but low stake battles between players
* Creative base building and raiding strategies

We want to improve upon these factors compared to BaseWars:



* Give players a clear sense of progression
* Encourage players to leave their base more by adding incentives
* Make combat more fair and more fun
* Create endgame goals and content


## Resources

The old BaseWars only relied on money and power. In order to encourage more base expansion and add end-game goals, we have created a small selection of resources that the player has to manage.

***Primary*** resources are most used and critical to a functioning base.



* **Money** is your main liquid currency used to buy things off-map, stored in your account. It is also spent when building things.
* **Supply** is what you use to build things. They are *physical containers* that you must call in with airdrops or be produced. There are three tiers of supplies, basic, advanced and experimental.
* **Energy** is produced by generators and used by machines. They are _non-physical_, stored in generators and batteries. Most things must be supplied with energy by being in range of a generator. Generators close to one another form a network to share energy with, and batteries can boost energy storage.

***Secondary*** resources are directly gathered, and can be used to produce or boost production of primary resources. They can be stored in depots, as physical containers, or on the player.



* **Scrap** is a common resource used to make basic **Supplies**. It can be found lying around at scrap deposits.
* **Metal** is harvested by mining machines from certain areas of the map. It is rarer than scrap, and can be used to create advanced **Supplies**.
* **Oil** is also harvested from specific areas on the map, but depletes over time. It can be used in generators to enhance **Energy **production.
* **Uranium** is mined from deposits. It can be used in high-tier generators to enhance **Energy **production.
* **Coolant** is gathered from tanks around the map or using mining machines. It is used to temporarily boost production of **Money**.
* **Parts** rarely spawn around the map, and can be used to permanently boost production of **Money** through upgrades.

There is one special resource, **Stardust**, that falls from the sky rarely. Stardust can be used to create experimental **supplies**, boost production of **energy**, or sold for a large sum of **money**.


## Mechanics


### Armor

Armor can be replenished at a dispenser and simply serves as an addition to your health. Health regenerates over time when not injured but Armor does not.


### Gear and Inventory

Players can carry up to two weapons and up to two gadgets. They can carry a limited amount of ammo and a limited amount of secondary resources.

Players always start with an engineering tool (E-Tool) and a blowtorch. The E-Tool is used to construct basic defenses. The blowtorch can damage hostile objects and repair friendly ones.

Players can carry Supply Crates and some light Machines in their hands. Doing this means they cannot use any weapons and will be slowed to a crawl.


### Defenses and Machines

All **machines** require power to operate. There is a small passive energy consumption drain as well as a larger amount of energy consumed when performing their duties. Energy is recharged/supplied by being in range of a machine connected to a generator. Generators work like a power grid, so they can share energy from other generators.

When a machine mines or collects something, it will look for nearby containers that it can be stored in. Otherwise, it will store it internally.

Some small machines are considered **deployables**. They can be picked up and moved around, and are usually made from the 3D Printer.

There are also **buildables** that aren’t considered machines, like walls and gates. Buildables and machines are usually called **defenses **together.

If a player disconnects, their defenses will remain for some time. After the time is up, they will get a 75% refund to their account, but only if they were not being raided at that time.


### Teaming and Raiding

By default, all players are neutral to each other. When one player attacks another player, they are set to be hostile for a period of time, during which base defenses will target them. However, if a raid is not in progress, defenses will take no damage (except sentries).

You can form a team by inviting or joining another person. Players on the same team are immune to team damage and team defenses will not target them. They will also be able to open doors owned by their team.

To raid an enemy player, the attacker must first own a Satellite Uplink machine, which allows them to target a defender to raid. A raid is only possible if the defender:



* Owns at least one claim beacon;
* Is not actively being raided by other attackers;
* Has not raided for some time (10 minutes);
* Have at most one less player than the attacker;
* Has earned a minimum amount of money;
* Has a base value exceeding a threshold.

During a raid, attackers are able to damage defenses the defenders own. They can use the blowtorch to slowly destroy any defense. Some defenses are bullet resistant, but blast damage is effective against almost all buildables. Destroying a machine gives the attacker a portion (75%) of its monetary value.

Defenders can also damage defenses the attackers own, but they do 50% less damage. They can choose to launch a counter-raid (same process as raiding normally) which will cancel out this penalty.

A raid lasts a certain amount of time (5 minutes). It may be prematurely canceled if the attacker chooses to, or if the defender’s total base value falls under the minimum threshold, both of which will trigger a 30 second grace before ending the raid. After a raid ends, the attacker cannot raid for some time (5 minutes).


### Infamy

Infamy points are permanently attached to your character. They can be gained in only one way: Your team must purchase and detonate a Thermonuclear Device. Only one Thermonuclear Device can exist at any one time, and it is only buyable when there are at least 6 players on the server.

Once the Thermonuclear Device is deployed, it takes 5 minutes to activate before it can detonate. During this time, all teams can attack the nuking team as if raiding. The countdown will be stopped if the bomb can be destroyed before it reaches 0.

If the bomb is successfully detonated, the player who bought it will receive 1 infamy point. Their money will also be reset. When the bomb is detonated, every structure on the server will be destroyed and every character will be killed.

For every 5 players outside of your team, you will receive 1 extra infamy point.

Infamy points can be used to purchase small, permanent upgrades to the player’s stats:



* +1% Max health
* +1% Max armor
* +1% Move speed
* +1% Jump power
* +1% Carrying capacity

To a maximum of 25% each.


#### Buying and Selling

Supplies and other items are purchased off-map. This means that an order is made and a plane is called in to airdrop these things to you. Other players can intercept your delivery if they reach it before you or interfere with the plane using certain emplacements.

Resources can be sold using either the Fulton Balloon or Merchant Teleporter. The Fulton Balloon can hold a certain amount of any resource and takes several seconds to rise and then get sold. Other players can shoot down the balloon in its early stages, which causes the resources within to scatter back down, allowing them to be recovered. The Merchant Teleporter** **instantly sells any resources it is given, but is a high level machine that takes a lot of energy to run.


## Content


### Map

Unlike the original BaseWars, our gamemode allows players to make bases both indoor and outdoor. Interior bases are protected from aerial attacks and have limited points of entry, but can’t house high end machines. Exterior bases are harder to defend, but allow more player freedom in choosing a site and structure. This should allow the gamemode to function on a variety of maps ranging from open to closed, but might not work best on maps with small, closed skyboxes and limited outdoor space.

Our vision of a first map for this gamemode is similar to that of the original rp_bangclaw. It will have a suburban area with small buildings scattered, and a much larger desert area with few buildings.


### Weapons

We plan to start with a small but rounded selection of these guns:



* Pistol - Starter weapon. (M9, MP-443 Grach, Glock 17)
* Submachine Gun - Sustained fire weapon with low damage but low recoil with a huge magazine. (PP-19 Bizon, P-90)
* Assault Rifle - Well-balanced all rounder weapon. (M4A1, AK-74)
* Marksman Rifle - Long range semi-automatic rifle for shooting at distant targets. (SVD, M14, M110)
* Pump Shotgun - Powerful, slow-firing shotgun. (KS-23)
* Rocket Launcher - Very powerful explosive that can be used to destroy structures or damage players. (RPG-7, SMAW, Carl Gustaav)


### Gadgets

Gadgets are all printed by the 3D printer.



* **Demolition Charge**: Deployable explosive C4 pack for destroying static defences. Has a timer before detonation, and can be shot or defused by enemies.
* **Fulton Balloon**: A balloon that sends away resources for sale.
* **Airdrop Beacon**: Marks an area for dropping bought supplies and items.
* **Laser Designator**: Marks a target for friendly artillery. The fire mode, artillery type, and battery can be selected.


### Building Pieces

These buildables serve as fundamental defenses in a base. You put it down, and things can’t get through. Most of them are resistant to bullet damage, and can only be broken by the blowtorch or explosives.


#### All Tiers



* **Interior Walls**: Small walls intended for indoor use. Has versions for door frames, window frames, chest-high and half width. Cost and durability dependent on material:
    * **Wood**: Very cheap and does not need supplies. Breaks easily.
    * **Scrap**: Costs a small amount of basic supplies.
    * **Concrete**: Costs some basic supplies and a tiny amount of advanced supplies.
    * **Reinforced**: Costs a decent amount of advanced supplies.
    * **Nanotech**: Costs experimental supplies, but is extremely durable.
* **Door**: Just a regular door. Takes damage depending on its tier: Wood, Scrap, Steel, Blast, Nanotech.


#### Tier 0

All Tier 0 buildings can be made using the E-Tool.	



* **Sandbags**: Waist-high stacks of bags of sand/dirt. Dirt cheap and bullet resistant.
* **Fences**: See-through barriers. Cheap and quick to build, but easily destroyed.
* **Fence Gate/Door**: A see-through gate and door that blocks access.


#### Tier 1



* **Bastions**: Neck high stackable blocks of sand. Slow to build but somewhat durable. Uses basic supplies.
* **Razor Wire**: Deployable spools of razor wire that slow and deal damage to enemies that walk through them.


#### Tier 2



* **Barriers**: Waist-high concrete barriers providing good cover. Uses advanced supplies.
* **T-Walls**: Tall concrete barriers. Expensive, but relatively fast to build and durable. Uses advanced supplies.
* **Watch Tower**: Tower that can be built in the open. Armored from light weapons fire.


#### Tier 3



* **Hexwalls**: Advanced exterior walls that cost Experimental Supplies to build. Extremely durable.
* **Particle Gates**: The fading doors of tomorrow. Two emitter poles form a wall of any size, and dealing damage to it will only make it go offline instead of being destroyed, recharging after some time and energy. One team can only have a limited amount of particle gates.


### Core

These buildables are vital to sustaining and developing a base.


#### Tier 0:



* **Claim Beacon**: Deployable that prevents other players from building in its radius.
* **Supply Depot**: Area for supplies or secondary resources to be stored. Built using E-Tool.
* **Nano-Printer**: 3D printing machine able to produce gear and small machines. Can be ordered for airdrop. Required to make Tier 1 buildings.


#### Tier 1:



* **Spawn Beacon**: Deployable that allows you to respawn from it. Consumes energy on respawn.
* **Armory Dispenser**: Deployable that resupplies ammo and armor.
* **Construction Gantry**: Huge crane that can unfold and build large structures. Required for the construction of Tier 2 buildings.
* **Scanner Uplink**: Allows you to scan other players and initiate a raid.


#### Tier 2:



* **Drone Hub**: Hive for drones that can build medium sized base objects in their radius. Required for making Tier 3 buildings. Drones can be shot down and take time and energy to rebuild. Each drone hub provides **2** drones.
* **Maintenance Module**: Allows drone hub drones to automatically repair friendly structures in range.
* **Aid Module**: Allows drone hub drones to resupply player ammo and armor in range.
* **Defense Module**: Allows drone hub drones to operate like weak strike drones, allowing them to “zap” players for a small amount of damage and stunning them briefly.


#### Tier 3:



* **Thermonuclear Device**: Costs a huge amount of money and a large amount of Experimental Supplies. When constructed and enabled, you are hostile to all other players for the duration of activation. See **Infamy **section for details.
* **Teleporter Pad**: Allows you to instantly transport yourself to any other teleporter pad you own.


### Power


#### Tier 0:



* **Pocket Generator**: Deployable generator. Produces a small amount of power.
* **Diesel Generator**: Basic generator. Can be boosted using oil.


#### Tier 1:



* **Gas Turbine Generator**: More powerful than diesel generator. Can be boosted using oil.
* **Solar Panels**: Large panels generating power from light. Requires unobstructed view to the sky.
* **Battery**: Deployable that stores a moderate amount of energy.


#### Tier 2:



* **Fission Reactor**: Large machine that produces huge amounts of power, boostable with uranium. Requires advanced supplies.
* **Wind Turbine**: Huge machine that produces power using wind. Production depends on the amount of clearance the blades have.
* **Ion Battery**: Stores a large amount of energy.


#### Tier 3:


* **Fusion Reactor**: Large machine that produces incredible amounts of power, boostable with uranium. Requires a lot of advanced supplies.
* **Singularity Reactor**: Produces the most amount of energy, boostable with stardust. Requires experimental supplies.
* **Black Hole Battery**: Stores a massive amount of energy.


### Production


#### All Tiers:



* **Bitcoin Miner**: Consume energy to produce money. Can be boosted temporarily using coolant and upgraded permanently using parts. Has many different levels, increasing in cost, production rate and energy consumption.
    * Tier 0: Scrap, Salvaged
    * Tier 1: Bronze, Silver, Gold, Platinum
    * Tier 2: Quartz, Ruby, Emerald, Diamond, Amethyst
    * Tier 3: Supercomputer, Hyperprocessor
    * Dimensional Processor: Mines BTC by pulling computation power from parallel universes.
    * Precognition Processor: Mines BTC in the future, then sends calculations back in time so they are complete before they were ever even submitted.


#### Tier 0:



* **Reserve Printer**: A special deployable money printer powered by an internal nuclear battery. It costs no power and is absolutely free (you keep it on you in case of an emergency), but you can only ever have one.


#### Tier 1:



* **Portable Drill**: Small deployable for mining metal and uranium.
* **Liquid Pump**: Small deployable for extracting liquids such as oil and coolant.
* **Packaging Machine**: Assembles supplies out of scrap, metal, and stardust.


#### Tier 2:



* **Deep Drill**: Large drill that excavates metal and uranium at a higher rate.
* **Oil Pumpjack**: Large oil harvesting machine.
* **Scrap Harvester Hub**: Deploys drones that seek and collect scrap. Doesn’t actually remove scrap from the world.


#### Tier 3:



* **Mass Fabricator**: Consumes huge amounts of energy to generate random resources, include stardust.
* **Merchant Teleporter**: Sells your goods directly via a teleportation device. Consumes energy to doing so.
* **Mineral Materializer**: Remateralize minerals straight out of the ground. Extremely efficient but consumes a large amount of energy.
* **Liquid Materializer**: Remateralize liquid deposits right into our tanks. Extremely efficient but consumes a large amount of energy.


### Emplacements

Buildable weapons for defensive and offensive use.


#### Tier 0:



* **Mini-sentry**: Deployable sentry with a 9mm SMG.
* **Landmine**: Deployable mine that deals heavy damage to players.


#### Tier 1:



* **HMG Sentry**: Equipped with a 12.7mm machine gun.
* **Mortar**: Automatic mortar system that can be fired at the enemy base. Has a 5 round magazine that recharges slowly over time. Capable of firing smoke rounds.
* **Arena System**: Deployable device that shoots down incoming projectiles. Has a 3 round magazine that recharges over time. Destroys all projectiles that come within a short range of the module.


#### Tier 2:



* **Grenade Sentry**: Equipped with an automatic grenade launcher.
* **Rocket Artillery**: Automatic MLRS that can be fired at enemies. Fires a burst of 20 rockets at once, and requires a long time to reload. Rockets are inaccurate and do poor damage. Can also fire incendiary rockets, which create a lasting fire effect.
* **Kashtan System**: High rate of fire air defense autocannon capable of shooting down incoming artillery projectiles at a fast rate. Non-teammates cannot call in airdrops within a moderate radius.
* **Minelayer**: System that deploys antipersonnel mines over a moderate radius.


#### Tier 3:



* **Laser Sentry**: Equipped with a high damage pulsed laser. Capable of shooting down incoming projectiles.
* **Strike Drone Hub**: Launches a squadron of small strike drones with three modes. Strike drones can be shot down with regular weapons, if one is accurate enough. Squadrons are made up of 3 drones. Drones take time to rearm after each sortie; if a drone is shot down, it will need to be rebuilt, which takes time. Drones will remain on patrol until all their ammunition is expended or they run out of fuel after 5 minutes.
    * Bombing mode allows the drones to drop bombs on their target. Strike drones carry one bomb each.
    * Patrol mode causes drones to circle their target and strafe enemy players or defenses. Strike drones carry an autocannon with 100 rounds each.
    * Fighter mode causes drones to patrol the target radius, preventing airdrops and attacking enemy strike drones that enter their path. Strike drones carry 2 air-air missiles.
* **Orbital Laser System**: Fire an extremely powerful laser beam at a network of spaceborne deflectors to fire at a target in range. Has a fairly small damage radius, however.


## Challenges and Concerns



* **Isn’t this a lot more like base building games like Rust?**
    * In our gamemode, building is confined to what the terrain allows - there is an intentional lack of pieces such as foundations and roofs. While players can still set up bases outdoors using walls, these bases are much more vulnerable compared to bases made using the map’s interiors. This is a tradeoff that gives the building process more depth and allows more opportunities for raiders to exploit weaknesses.
    * In addition, our gamemode has deliberately lower stakes compared to games like Rust, and much of the design prevents more invested players from snowballing others while still making progression meaningful.
* **Doesn’t BaseWars have rules for raiding and RDM, among other things?**
    * Basewars only has rules as a remnant of its past as an offshoot of an RP gamemode. We believe that admin-enforced rules are unnecessary, though servers wishing to enforce them regardless still can do so.
    * Automated systems enforce raiding rules that were previously handled by admins - this eliminates ambiguity and eases admin workload.
    * Deathmatching is an expected occurrence now that the map contains more resources, so RDMing isn’t really a thing anymore.
    * Turrets are destructible even not during raids, making KOS lines unneeded.
* **This proposal feels too dissimilar to BaseWars.**
    * It’s almost certain there will be more faithful recreations of BaseWars, but this gamemode isn’t trying to do that. We hope to preserve the core appeal of BaseWars while giving it more depth, which inevitably means changing things up.
    * A lot of the game will still revolve around building a base, raiding and getting raided; but you can choose to do many more activities in the middle, and the extra mechanics serve to add both diversity and depth. 
    * As we develop and playtest, we will tune the gamemode to achieve this goal. Nothing on this document is final.
* **This is a lot of content, can you really handle all this?**
    * Our team has plenty of experience working with Garry’s Mod and overall do not believe that the technical hurdles of this concept will be insurmountable.
    * The major challenge is the large amount of assets required for entities. While our team has modellers, texture artists and sound designers, it will take a long time before all the required assets are made. Before then, we will develop using placeholder assets from sources like GameBanana or other games.
