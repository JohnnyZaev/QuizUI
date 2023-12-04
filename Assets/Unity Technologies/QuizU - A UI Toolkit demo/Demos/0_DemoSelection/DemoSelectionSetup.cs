
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Quiz
{
    /// <summary>
    /// This utility class sets up the DemoSelectionScreen and connects it with the DemoSelectionPresenter
    /// for the MVP (Model-View-Presenter) pattern. This is a MonoBehaviour to use the Unity lifecycle/PlayerLoop
    /// and to communicate with other objects.
    /// </summary>

    public class DemoSelectionSetup : MonoBehaviour
    {
        [Tooltip("The UI Document which contains the visual tree ")]
        [SerializeField] UIDocument m_Document;

        DemoSelectionPresenter m_DemoSelectionPresenter;
        DemoSelectionScreen m_DemoSelectionScreen;

        const string k_ScenePathToUnload = "Assets/Demos/0_DemoSelection/DemoSelection.unity";

        private void OnEnable()
        {
            if (m_Document == null)
                m_Document = GetComponent<UIDocument>();

            if (m_DemoSelectionPresenter == null)
                m_DemoSelectionPresenter = GetComponent<DemoSelectionPresenter>();

            NullRefChecker.Validate(this);

            // Initialize helper utility for non-MonoBehaviours to run coroutines

            if (!Coroutines.IsInitialized)
            {
                Debug.Log("Initializing coroutines");
                Coroutines.Initialize(this);
            }            

            SetupDemoSelectionScreen();

            DemoEvents.BackButtonClicked += DemoEvents_BackButtonClicked;

        }


        // Dispose of the DemoSelectionScreen when disabling
        private void OnDisable()
        {
            // Unregister events and disable the screen
            DemoEvents.BackButtonClicked -= DemoEvents_BackButtonClicked;

            m_DemoSelectionScreen.Disable();
        }

        // Initialize and configure the demo selection screen
        private void SetupDemoSelectionScreen()
        {
            VisualElement rootElement = m_Document.rootVisualElement.Q<VisualElement>("demo-select-container");

            m_DemoSelectionScreen = new DemoSelectionScreen(rootElement, m_DemoSelectionPresenter.DemoInfo);

            // Create EventRegistry and set initial states
            m_DemoSelectionScreen.Initialize();

            // Create Buttons on NavigationBar
            m_DemoSelectionScreen.SetupNavigationBar(m_DemoSelectionPresenter.DemoInfo.Length);

            // Register button actions
            m_DemoSelectionScreen.RegisterCallbacks();

            // Show UI and highlight the first button by default
            m_DemoSelectionScreen.NavigationBar.HighlightButton(0);
            m_DemoSelectionScreen.Show();

            // Assign new View/UI to Presenter
            m_DemoSelectionPresenter.DemoSelectionScreen = m_DemoSelectionScreen;

            // Notify other components UI is ready
            DemoEvents.SetupCompleted?.Invoke();
        }

        // Unload the DemoSelectionScene and go back to the MainMenu
        private void DemoEvents_BackButtonClicked()
        {
            // Note that this is handled slightly differently from the DemoScenes.This unloads the specific
            // DemoSelectionScene by scene path
            SceneEvents.UnloadSceneByPath?.Invoke(k_ScenePathToUnload);
        }

    }
}
