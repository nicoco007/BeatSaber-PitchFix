// <copyright file="AudioPatches.cs" company="Nicolas Gnyra">
// This file is part of PitchFix.
//
// PitchFix is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// PitchFix is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with PitchFix.  If not, see https://www.gnu.org/licenses/.
// </copyright>

using System;
using System.IO;
using System.Reflection;
using PitchFix.Utilities;
using SiraUtil.Affinity;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;
using Object = UnityEngine.Object;

namespace PitchFix.Patches;

internal class AudioPatches : IAffinity, IInitializable, IDisposable
{
    private AudioMixer _audioMixer;

    public async void Initialize()
    {
        try
        {
            using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("PitchFix.Resources.Assets");
            AssetBundleCreateRequest assetBundleCreateRequest = await AssetBundle.LoadFromStreamAsync(stream);
            AssetBundle assetBundle = assetBundleCreateRequest.assetBundle;

            if (assetBundle == null)
            {
                return;
            }

            AssetBundleRequest assetRequest = await assetBundle.LoadAssetAsync<AudioMixer>("Assets/PitchFix.mixer");
            _audioMixer = (AudioMixer)assetRequest.asset;

            await assetBundle.UnloadAsync(false);
        }
        catch (Exception ex)
        {
            Plugin.log.Error(ex);
        }
    }

    public void Dispose()
    {
        Object.Destroy(_audioMixer);
    }

    [AffinityPrefix]
    [AffinityPatch(typeof(AudioTimeSyncController), nameof(AudioTimeSyncController.Awake))]
    private void AudioTimeSyncController_Awake(AudioTimeSyncController __instance)
    {
        if (_audioMixer == null)
        {
            Plugin.log.Error("Loaded audio mixer is null!");
            return;
        }

        _audioMixer.outputAudioMixerGroup = __instance._audioSource.outputAudioMixerGroup;
        __instance._audioSource.outputAudioMixerGroup = _audioMixer.FindMatchingGroups("Master")[0];
    }

    [AffinityPrefix]
    [AffinityPatch(typeof(AudioManagerSO), nameof(AudioManagerSO.musicPitch), AffinityMethodType.Setter)]
    private void AudioManagerSO_musicPitch(float value)
    {
        if (_audioMixer == null)
        {
            Plugin.log.Error("Loaded audio mixer is null!");
            return;
        }

        _audioMixer.SetFloat("MusicPitch", value);
        if (Mathf.Approximately(value, 1f))
        {
            _audioMixer.SetFloat("MusicPitchShifterWet", -100f);
        }
        else
        {
            _audioMixer.SetFloat("MusicPitchShifterWet", 0f);
        }
    }
}
