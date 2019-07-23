Pro Pooling

Thank you for using Pro Pooling. 

If you have any thoughts, comments, suggestions or otherwise then please contact us through our website or 
drop me a mail directly on mark_a_hewitt@yahoo.co.uk

Please consider rating this asset on the asset store.

Regards,
Mark Hewitt

For more details please visit: http://www.flipwebapps.com/unity-assets/pro-pooling/

- - - - - - - - - -

QUICK START

1. If you have an older version installed:
  1.1. Make a backup of your project
  1.2. Delete the old /FlipWebApps/ProPooling folder to cater for possible conflicts.
2. Import the asset.
3. Check out the demo scenes under /FlipWebApps/ProPooling/_Demo
4. For tutorials visit http://www.flipwebapps.com/unity-assets/pro-pooling/

- - - - - - - - - -

CHANGE LOG

v3.1.1
	- Fixes for deprecations in Unity 2018.3
	- Minimum tested version bumped to 2017.1

v3.1
    - General: Minimum tested version bumped to 5.6
    - GlobalPools / Pools: Added GlobalPool and Pools DespawnAll() and DespawnAllPools methods
	- Pools: Added progressive filling of the pools
	- Pools: Allow recycling of oldest item when limits reached
	- Spawner: Added Lifecycle Events and GameActions to Spawner Component
	- Spawner: Added looping and bursts

v3.0
This version is a major update that contains many of the most requested updates and improvements. There are
several API changes that have been needed to accommodate this (see below for full details) and so this version is
not directly backwards compatible, although the interface is still similar to before.. 

	- Demos: Increased timer accuracy in speed demos
	- IPoolComponent: OnGetFromPool -> OnSpawned, OnReturnToPool -> OnDespawned
	- Pool: PreInitialiseCount -> InitialSize, MaxCapacity -> MaximumSize
	- Pool: GetFromPool -> Spawn, GetPoolItemFromPool -> SpawnPoolItem, ReturnToPool -> Despawn
	- Pool: Pools are not automatically initialised from the constructor. Call pool.Initialise() to preallocate items.
	- PoolItem: OnGetFromPool -> OnSpawn, OnReturnToPool -> OnDespawn, ReturnSelf -> DespawnSelf
	- PoolItem Components: PoolEventEnableChildren -> OnSpawnEnableChildren
	- PoolItem Components: PoolEventResetRigidbody -> OnSpawnResetRigidbody
	- PoolItem Components: PoolEventResetRigidbody2D -> OnSpawnResetRigidbody2D
	- PoolItem Components: ReturnToPoolAfterDelay -> DespawnAfterDelay
	- PoolItem Components: New DespawnAfterCollision component for despawning after a collision.
	- PoolItem Components: New DespawnAfterCollision2D component for despawning after a 2D collision.
	- PoolItem Components: New OnDespawnResetTransform component for resetting transform of despawned items.
	- Pools: New Pools component for adding per scene / non persistant pools
	- Game Framework: New PoolItemComponent for running Actions on Spawn / Despawn
	- Game Framework: Added new StartSpawning, StopSpawning, PauseSpawning, ResumeSpawning, Spawn Burst, Spawner and Despawn GameActions
	- GlobalPools: PoolManager is renamed to GlobalPools.
	- GlobalPools: PoolManager is always a singleton (use AddPools) component to add per scene pools.
	- GlobalPools: PoolManager is always a singleton (use AddPools) component to add per scene pools.
	- GlobalPools: Improved editor
	- GlobalPools: Default options and behaviour are now configurable.
	- GlobalPools: Reporting to show run time usage.
	- GlobalPools: GetFromPool -> Spawn, GetPoolItemFromPool -> SpawnPoolItem, ReturnToPool -> Despawn
	- Spawning: New Spawner component for automatically spawning pool items.
	- Spawning: New spawning demo.

v2.1

	- Poolmanager: Updated editor window allowing for drag and drop of prefabs to create new pools
	- PoolManager: Check for null prefabs
	- Tests: Fixes for warnings when running editor tests.
	- Removed unused EditorList class

v2.0
NOTE: This version contains some API changes and may require minor code changes. See below for details. If upgrading you will likely need to delete the 
old /FlipWebApps/ProPooling folder to cater for possible conflicts.

	Improvements
	- Components: IPoolComponent interface methods now have PoolItem as a parameter. Add this to any custom implementations.
	- Components: Added ReturnToPoolAfterDelay to automatically return pool items to their pool after a specified delay.
	- Components: changed namespace from FlipWebApps.ProPooling.Scripts.Components to ProPooling.Components
	- Demo: Added Auto return to pool after delay demo.
	- Pooling: changed namespace for pooling classed from FlipWebApps.ProPooling.Scripts to ProPooling
	- Pool: Updated Documentation
	- Pool: Added Name property
	- Pool: ID automatically returns Prefab Instance ID
	- Pool: Created pool items have same name as prefab
	- Pool: Pool is no longer declared as a generic type for simplicity. Rename references of Pool<PoolItem> to Pool. See also PoolGeneric.
	- Pool: Added PoolGeneric class that inherits from Pool for use with custom PoolItem derived classes.
	- Pool: IReturnToPool is removed. Just reference Pool instead.
	- PoolManager: Methods are no longer static and must be accessed through PoolManager.Instance.
	- PoolManager: Allow referencing pools by name
	- PoolManager: Allow adding new pools for management
	- PoolManager: Check for pools with missing prefab before setup
	- PoolManager: Added GetFromPool, GetPoolItemFromPool and ReturnToPool methods to indirectly get and return items from / to the different pools.

v1.1
	Improvements
	- New graphical demo showing generics, and delegated pool returns.
	- PoolItem lifecycle methods renamed - prefixed with 'On'
	- Added IReturnToPool interface with reference in PoolItem and ReturnSelf() method.
	- Pool - some items reworked around PoolItem to enable delegated references.
	- Added custom pool inspector.

v1.0
	First public release