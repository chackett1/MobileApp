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

using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProPooling._Demo
{

    public class SpawningDemo : MonoBehaviour
    {
        public GameObject MissingScenes;
        public string[] Scenes = { "1.1 Spawning Demo - First Scene", "1.2 Spawning Demo - Second Scene", "1.3 Spawning Demo - Third Scene", "1.4 Spawning Demo - Fourth Scene", "1.5 Spawning Demo - Fifth Scene", "1.6 Spawning Demo - Sixth Scene" };
        public Vector3[] ScenePositions;
        public float[] TransitionTimes;

        public static SpawningDemo Instance { get; set; }

        int _currentScene = 0;
        bool isChangingScene = false;

        private void Awake()
        {
            Instance = this;

            // Verify all scenes are added to the build settings.
#if UNITY_EDITOR
            var foundScenes = 0;
            var scenes = UnityEditor.EditorBuildSettings.scenes;
            foreach(var scene in scenes)
            {
                foreach(var usedScene in Scenes)
                    if (scene.path.Contains(usedScene))
                        foundScenes++;
            }
            if (foundScenes < Scenes.Length)
            {
                MissingScenes.SetActive(true);
                Debug.LogWarning("Please add all Pro Pooling demo scenes into your build settings so that the demo works as expected.");
            }
#endif

            SceneManager.LoadScene(Scenes[_currentScene], LoadSceneMode.Additive);
        }

        //private void OnEnable()
        //{
        //}

        //private void OnDisable()
        //{           
        //}

        public void LoadNextScene(Animator currentSceneAnimator)
        {
            StartCoroutine(LoadDemoSceneAsync(_currentScene + 1, currentSceneAnimator));
        }


        public void LoadPreviousScene(Animator currentSceneAnimator)
        {
            StartCoroutine(LoadDemoSceneAsync(_currentScene - 1, currentSceneAnimator));
        }


        System.Collections.IEnumerator LoadDemoSceneAsync(int newSceneIndex, Animator currentSceneAnimator)
        {
            if (isChangingScene) yield break;
            isChangingScene = true;

            currentSceneAnimator.SetBool("Shown", false);

            var currentSceneName = Scenes[_currentScene];
            var sceneName = Scenes[newSceneIndex];
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            //Wait until the last operation fully loads to return anything
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
            _currentScene = newSceneIndex;

            // Move to new position
            var elapsedTime = 0f;
            var startingPosition = Camera.main.transform.position;
            while (elapsedTime < TransitionTimes[_currentScene])
            {
                Camera.main.transform.position = Vector3.Lerp(startingPosition, ScenePositions[_currentScene], (elapsedTime / TransitionTimes[_currentScene]));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

#if UNITY_5_5_OR_NEWER
            SceneManager.UnloadSceneAsync(currentSceneName);
#else
            SceneManager.UnloadScene(currentSceneName);
#endif

            isChangingScene = false;
        }


        public void ShowRatePage()
        {
            Application.OpenURL("https://www.assetstore.unity3d.com/#!/content/59286?aid=1011lGnE");
        }
    }
}