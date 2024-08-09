// <copyright file="AppInstaller.cs" company="Nicolas Gnyra">
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
using PitchFix.Patches;
using SiraUtil.Affinity;
using Zenject;

namespace PitchFix;

internal class AppInstaller : Installer
{
    public override void InstallBindings()
    {
        Container.Bind(typeof(IAffinity), typeof(IInitializable), typeof(IDisposable)).To<AudioPatches>().AsSingle();
    }
}
