﻿using System;
using Unity.Cloud.Common;
using Unity.ReferenceProject.ScenesList;
using Unity.ReferenceProject.StateMachine;
using Unity.ReferenceProject.DataStores;
using Unity.ReferenceProject.DeepLinking;
using UnityEngine;
using Zenject;

namespace Unity.ReferenceProject
{
    [Serializable]
    public sealed class SceneSelectionState : AppState
    {
        [SerializeField]
        AppState m_SceneSelectedState;

        [SerializeField]
        SceneListUIController m_SceneListUIController;

        PropertyValue<IScene> m_ActiveScene;
        
        DeepLinkData m_DeepLinkData;

        [Inject]
        void Setup(PropertyValue<IScene> activeScene, DeepLinkData deepLinkData)
        {
            m_ActiveScene = activeScene;
            m_DeepLinkData = deepLinkData;
        }

        void Awake()
        {
            m_SceneListUIController.ProjectSelected += OnProjectSelected;
        }

        protected override void EnterStateInternal()
        {
            m_SceneListUIController.Refresh();
        }

        void OnProjectSelected(IScene scene)
        {
            Debug.Log($"Selected scene '{scene.Name}'");
            m_DeepLinkData.SetDeepLinkCamera = false;
            AppStateController.PrepareTransition(m_SceneSelectedState).OnBeforeEnter(() => m_ActiveScene.SetValue(scene)).Apply();
        }
    }
}
