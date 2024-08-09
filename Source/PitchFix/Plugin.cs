// <copyright file="Plugin.cs" company="Nicolas Gnyra">
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

using IPA;
using IPA.Logging;
using SiraUtil.Zenject;

namespace PitchFix;

[Plugin(RuntimeOptions.DynamicInit)]
public class Plugin
{
    [Init]
    public Plugin(Logger logger, Zenjector zenjector)
    {
        log = logger;

        zenjector.Install<AppInstaller>(Location.App);
    }

    internal static Logger log { get; private set; }
}
