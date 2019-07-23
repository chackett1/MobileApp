//----------------------------------------------
// Flip Web Apps: Pro Pooling
// Copyright © 2016-2017 Flip Web Apps / Mark Hewitt
//
// Please direct any bugs/comments/suggestions to http://www.flipwebapps.com
// 
// The copyright owner grants to the end user a non-exclusive, worldwide, and perpetual license to this Asset
// to integrate only as incorporated and embedded components of electronic games and interactive media and 
// distribute such electronic game and interactive media. End user may modify Assets. End user may otherwise 
// not reproduce, distribute, sublicense, rent, lease or lend the Assets. It is emphasized that the end 
// user shall not be entitled to distribute or transfer in any way (including, without, limitation by way of 
// sublicense) the Assets in any other way than as integrated components of electronic games and interactive media. 

// The above copyright notice and this permission notice must not be removed from any files.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//----------------------------------------------

#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2
#else
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace ProPooling.Editor.Tests
{
    public class PoolTests {
        #region Helper Functions
        #endregion Helper Functions

        bool AreAllChildrenActive(Transform parent)
        {
            for (var i = 0; i < parent.childCount; i++)
                if (parent.GetChild(i).gameObject.activeSelf == false)
                    return false;
            return true;
        }

        bool AreAllChildrenInactive(Transform parent)
        {
            for (var i = 0; i < parent.childCount; i++)
                if (parent.GetChild(i).gameObject.activeSelf == true)
                    return false;
            return true;
        }
        #region Initialisation and Cleanup

        [TestCase("Test 1")]
        [TestCase("Test 2")]
        public void PoolId(string name)
        {
            // Arrange
            var testGameObject = new GameObject(name);

            // Act
            var pool = new Pool(testGameObject);
            pool.Initialise();

            // Assert
            Assert.AreEqual(pool.ID, testGameObject.GetInstanceID(), "The Pool Id is not set correctly");

            pool.ClearPool();
            UnityEngine.Object.DestroyImmediate(testGameObject);
        }


        [TestCase("Test 1")]
        [TestCase("Test 2")]
        public void PoolName(string name)
        {
            // Arrange
            var testGameObject = new GameObject(name);

            // Act
            var pool = new Pool(testGameObject);
            pool.Initialise();

            // Assert
            Assert.AreEqual(pool.Name, testGameObject.name, "The Pool Name is not set correctly");

            pool.ClearPool();
            UnityEngine.Object.DestroyImmediate(testGameObject);
        }


        [Test]
        public void ConstructorCreatesEmptyPool()
        {
            // Arrange
            var testGameObject = new GameObject("test");
            var parentGameObject = new GameObject("parent");

            // Act
            var pool = new Pool(testGameObject);
            pool.Initialise();

            // Assert
            Assert.AreEqual(0, pool.InactiveCount, "Number of initial prefabs does not meet the counter");

            pool.ClearPool();
            UnityEngine.Object.DestroyImmediate(testGameObject);
            UnityEngine.Object.DestroyImmediate(parentGameObject);
        }


        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        public void InitialSizeAllocated(int count)
        {
            // Arrange
            var testGameObject = new GameObject("test");
            var parentGameObject = new GameObject("parent");

            // Act
            var pool = new Pool(testGameObject, initialSize: count, itemParent: parentGameObject.transform);
            pool.Initialise();

            // Assert
            Assert.AreEqual(count, pool.InactiveCount, "Number of initial prefabs does not meet the counter");

            pool.ClearPool();
            UnityEngine.Object.DestroyImmediate(testGameObject);
            UnityEngine.Object.DestroyImmediate(parentGameObject);
        }


        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        public void IsInitialisedNotSetBeforeInitialised(int count)
        {
            // Arrange
            var testGameObject = new GameObject("test");
            var parentGameObject = new GameObject("parent");

            // Act
            var pool = new Pool(testGameObject, initialSize: count, itemParent: parentGameObject.transform);

            // Assert
            Assert.False(pool.IsInitialised, "The pool should not be initialised before Initialise is called.");

            pool.ClearPool();
            UnityEngine.Object.DestroyImmediate(testGameObject);
            UnityEngine.Object.DestroyImmediate(parentGameObject);
        }


        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        public void IsInitialisedSetAfterInitialised(int count)
        {
            // Arrange
            var testGameObject = new GameObject("test");
            var parentGameObject = new GameObject("parent");

            // Act
            var pool = new Pool(testGameObject, initialSize: count, itemParent: parentGameObject.transform);
            pool.Initialise();

            // Assert
            Assert.True(pool.IsInitialised, "The pool should not be initialised after Initialise is called.");

            pool.ClearPool();
            UnityEngine.Object.DestroyImmediate(testGameObject);
            UnityEngine.Object.DestroyImmediate(parentGameObject);
        }


        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        public void InactiveItemsCreatedOnParent(int count)
        {
            // Arrange
            var testGameObject = new GameObject("test");
            var parentGameObject = new GameObject("parent");

            // Act
            var pool = new Pool(testGameObject, initialSize: count, itemParent: parentGameObject.transform);
            pool.Initialise();

            // Assert
            Assert.AreEqual(count, parentGameObject.transform.childCount, "Number of initial prefabs does not meet the counter");

            pool.ClearPool();
            UnityEngine.Object.DestroyImmediate(testGameObject);
            UnityEngine.Object.DestroyImmediate(parentGameObject);
        }


        [Test]
        public void ClearPoolDeletedItems()
        {
            // Arrange
            var testGameObject = new GameObject("test");
            var parentGameObject = new GameObject("parent");
            var pool = new Pool(testGameObject, initialSize: 5);
            pool.Initialise();

            // Act
            pool.ClearPool();

            // Assert
            Assert.AreEqual(0, pool.InactiveCount, "Pool items were not cleared.");

            UnityEngine.Object.DestroyImmediate(testGameObject);
            UnityEngine.Object.DestroyImmediate(parentGameObject);
        }

        #endregion Initialisation and Cleanup

        #region Spawn

        public void SpawnReturnsPreAllocatedInstance()
        {
            // Arrange
            var testGameObject = new GameObject("test");
            var parentGameObject = new GameObject("parent");
            var pool = new Pool(testGameObject, initialSize: 1, itemParent: parentGameObject.transform);
            pool.Initialise();

            // Act
            var gameObject = pool.Spawn();

            // Assert
            Assert.IsTrue(gameObject == testGameObject.transform.GetChild(0).gameObject, "Number of initial prefabs does not meet the counter");

            pool.ClearPool();
            UnityEngine.Object.DestroyImmediate(testGameObject);
            UnityEngine.Object.DestroyImmediate(parentGameObject);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        public void SpawnReturnesPrefab(int count)
        {
            // Arrange
            var testGameObject = new GameObject("test");
            var parentGameObject = new GameObject("parent");
            var pool = new Pool(testGameObject, initialSize: 5);
            pool.Initialise();

            // Act
            for (var i = 0; i < count; i++)
                pool.Spawn();

            // Assert
            Assert.AreEqual(5 - count, pool.InactiveCount, "Number of remaining pool items is not correct.");

            pool.ClearPool();
            UnityEngine.Object.DestroyImmediate(testGameObject);
            UnityEngine.Object.DestroyImmediate(parentGameObject);
        }

        [Test]
        public void SpawnFromEmptyPoolDefaultSettingsReturnesPrefab()
        {
            // Arrange
            var testGameObject = new GameObject("test");
            var parentGameObject = new GameObject("parent");
            var pool = new Pool(testGameObject);
            pool.Initialise();

            // Act
            var gameobject = pool.Spawn();

            // Assert
            Assert.IsNotNull(gameobject, "Did not return a gameobject.");
            Assert.IsTrue(pool.IsSpawnedInstance(gameobject), "gameobject should be in the pool inuse queue.");

            pool.ClearPool();
            UnityEngine.Object.DestroyImmediate(testGameObject);
            UnityEngine.Object.DestroyImmediate(parentGameObject);
        }

        #endregion Spawn
        #region Spawn SizeExceededModeType

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        public void SpawnReturnNull(int initialSize)
        {
            // Arrange
            var testGameObject = new GameObject("test");
            var pool = new Pool(testGameObject, initialSize, Pool.SizeExceededModeType.ReturnNull);
            pool.Initialise();

            // Act
            // empty pool
            for (var i = 0; i < initialSize; i++)
                pool.Spawn();
            var gameobject = pool.Spawn();

            // Assert
            Assert.IsNull(gameobject, "gameobject should have been null.");

            pool.ClearPool();
            UnityEngine.Object.DestroyImmediate(testGameObject);
        }


        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        public void SpawnCreateNonPoolItems(int initialSize)
        {
            // Arrange
            var testGameObject = new GameObject("test");
            var pool = new Pool(testGameObject, initialSize, Pool.SizeExceededModeType.CreateNonPoolItems);
            pool.Initialise();

            // Act
            // empty pool
            for (var i = 0; i < initialSize; i++)
                pool.Spawn();
            var gameobject = pool.Spawn();

            // Assert
            Assert.IsFalse(pool.IsSpawnedInstance(gameobject), "gameobject should not be in the pool inuse queue.");

            pool.ClearPool();
            UnityEngine.Object.DestroyImmediate(testGameObject);
        }


        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        public void SpawnIncreasePoolSize(int initialSize)
        {
            // Arrange
            var testGameObject = new GameObject("test");
            var pool = new Pool(testGameObject, initialSize, Pool.SizeExceededModeType.IncreasePoolSize);
            pool.Initialise();

            // Act
            // empty pool
            for (var i = 0; i < initialSize; i++)
                pool.Spawn();
            var gameobject = pool.Spawn();

            // Assert
            Assert.IsTrue(pool.IsSpawnedInstance(gameobject), "gameobject should be in the pool inuse queue.");

            pool.ClearPool();
            UnityEngine.Object.DestroyImmediate(testGameObject);
        }


        [TestCase(1, 5)]
        [TestCase(2, 10)]
        [TestCase(4, 20)]
        public void SpawnIncreasePoolSizeNoMaximum(int initialSize, int amountToGet)
        {
            // Arrange
            var testGameObject = new GameObject("test");
            var pool = new Pool(testGameObject, initialSize, Pool.SizeExceededModeType.IncreasePoolSize);
            pool.Initialise();

            // Act / Assert
            for (var m = 0; m < amountToGet; m++)
            {
                var gameobject = pool.Spawn();
                Assert.IsTrue(pool.IsSpawnedInstance(gameobject), "gameobject should be in the pool inuse queue.");
            }

            pool.ClearPool();
            UnityEngine.Object.DestroyImmediate(testGameObject);
        }


        [TestCase(1, 5, 10)]
        [TestCase(2, 10, 15)]
        [TestCase(4, 20, 25)]
        public void SpawnIncreasePoolSizeMaximum(int initialSize, int maximumSize, int amountToGet)
        {
            // Arrange
            var testGameObject = new GameObject("test");
            var pool = new Pool(testGameObject, initialSize, Pool.SizeExceededModeType.IncreasePoolSize, true, maximumSize);
            pool.Initialise();

            // Act / Assert
            // empty pool
            for (var m = 0; m < maximumSize; m++)
            {
                var gameobject = pool.Spawn();
                Assert.IsTrue(pool.IsSpawnedInstance(gameobject), "gameobject should be in the pool inuse queue.");
            }
            for (var e = maximumSize; e < amountToGet; e++)
            {
                var gameobject = pool.Spawn();
                Assert.IsNull(gameobject, "gameobject should have been null.");
            }

            pool.ClearPool();
            UnityEngine.Object.DestroyImmediate(testGameObject);
        }


        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        public void SpawnIncreasePoolSizeRecycleOldest(int initialSize)
        {
            // Arrange
            var testGameObject = new GameObject("test");
            var pool = new Pool(testGameObject, initialSize, Pool.SizeExceededModeType.RecycleOldest);
            pool.Initialise();

            // Act
            // empty pool
            var oldestGameObject = pool.Spawn();
            for (var i = 0; i < initialSize - 1; i++)
                pool.Spawn();

            var gameobject = pool.Spawn();

            // Assert
            Assert.AreEqual(oldestGameObject.GetInstanceID(), gameobject.GetInstanceID(), "gameobjects should be the same.");

            pool.ClearPool();
            UnityEngine.Object.DestroyImmediate(testGameObject);
        }


        [TestCase(1, 5, 10)]
        [TestCase(2, 10, 15)]
        [TestCase(4, 20, 25)]
        public void SpawnIncreasePoolSizeMaximumCreateIfMaximumExceeded(int initialSize, int maximumSize, int amountToGet)
        {
            // Arrange
            var testGameObject = new GameObject("test");
            var pool = new Pool(testGameObject, initialSize, Pool.SizeExceededModeType.IncreasePoolSize, true, maximumSize, Pool.MaximumSizeExceededModeType.CreateNonPoolItems);
            pool.Initialise();

            // Act / Assert
            // empty pool
            for (var m = 0; m < maximumSize; m++)
            {
                var gameobject = pool.Spawn();
                Assert.IsTrue(pool.IsSpawnedInstance(gameobject), "gameobject should be in the pool inuse queue.");
            }
            for (var e = maximumSize; e < amountToGet; e++)
            {
                var gameobject = pool.Spawn();
                Assert.IsFalse(pool.IsSpawnedInstance(gameobject), "gameobject should not be in the pool inuse queue. " + e);
            }

            pool.ClearPool();
            UnityEngine.Object.DestroyImmediate(testGameObject);
        }


        [TestCase(3)]
        [TestCase(2)]
        [TestCase(4)]
        public void SpawnIncreasePoolSizeMaximumRecycleOldest(int maximumSize)
        {
            // Arrange
            var testGameObject = new GameObject("test");
            var pool = new Pool(testGameObject, 0, Pool.SizeExceededModeType.IncreasePoolSize, true, maximumSize, Pool.MaximumSizeExceededModeType.RecycleOldest);
            pool.Initialise();

            // Act
            // empty pool
            var oldestGameObject = pool.Spawn();
            for (var i = 0; i < maximumSize - 1; i++)
                pool.Spawn();

            var gameobject = pool.Spawn();

            // Assert
            Assert.AreEqual(oldestGameObject.GetInstanceID(), gameobject.GetInstanceID(), "gameobjects should be the same.");

            pool.ClearPool();
            UnityEngine.Object.DestroyImmediate(testGameObject);
        }
        #endregion Spawn SizeExceededModeType

        #region Despawn

        [TestCase(5, 5)]
        [TestCase(10, 10)]
        [TestCase(20, 20)]
        public void DespawnCounterIsCorrect(int initialSize, int spawnCount)
        {
            // Arrange
            var testGameObject = new GameObject("test");
            var parentGameObject = new GameObject("parent");
            var pool = new Pool(testGameObject, initialSize: initialSize, itemParent: parentGameObject.transform);
            pool.Initialise();

            var spawnedGameObjects = new List<GameObject>();
            for (var i = 0; i < spawnCount; i++)
                spawnedGameObjects.Add(pool.Spawn());

            // Act
            while (spawnedGameObjects.Count != 0)
            {
                var spawnedGameObject = spawnedGameObjects[0];
                pool.Despawn(spawnedGameObject);
                spawnedGameObjects.RemoveAt(0);
            }

            // Assert
            Assert.True(AreAllChildrenInactive(parentGameObject.transform), "All items should be inactive.");

            pool.ClearPool();
            UnityEngine.Object.DestroyImmediate(testGameObject);
            UnityEngine.Object.DestroyImmediate(parentGameObject);
        }


        [TestCase(5, 5)]
        [TestCase(10, 10)]
        [TestCase(20, 20)]
        public void DespawnIsDisabled(int initialSize, int spawnCount)
        {
            // Arrange
            var testGameObject = new GameObject("test");
            var parentGameObject = new GameObject("parent");
            var pool = new Pool(testGameObject, initialSize: initialSize, itemParent: parentGameObject.transform);
            pool.Initialise();

            var spawnedGameObjects = new List<GameObject>();
            for (var i = 0; i < spawnCount; i++)
                spawnedGameObjects.Add(pool.Spawn());

            // Act
            while (spawnedGameObjects.Count != 0)
            {
                var spawnedGameObject = spawnedGameObjects[0];
                pool.Despawn(spawnedGameObject);
                spawnedGameObjects.RemoveAt(0);
            }

            // Assert
            Assert.True(AreAllChildrenInactive(parentGameObject.transform), "All items should be inactive.");

            pool.ClearPool();
            UnityEngine.Object.DestroyImmediate(testGameObject);
            UnityEngine.Object.DestroyImmediate(parentGameObject);
        }



        [TestCase(5, 5)]
        [TestCase(10, 10)]
        [TestCase(20, 20)]
        public void DespawnAll(int initialSize, int spawnCount)
        {
            // Arrange
            var testGameObject = new GameObject("test");
            var parentGameObject = new GameObject("parent");
            var pool = new Pool(testGameObject, initialSize: initialSize, itemParent: parentGameObject.transform);
            pool.Initialise();

            for (var i = 0; i < spawnCount; i++)
                pool.Spawn();

            // Act
            pool.DespawnAll();

            // Assert
            Assert.True(AreAllChildrenInactive(parentGameObject.transform), "All items should be inactive.");

            pool.ClearPool();
            UnityEngine.Object.DestroyImmediate(testGameObject);
            UnityEngine.Object.DestroyImmediate(parentGameObject);
        }
        
        #endregion Despawn
    }
}
#endif