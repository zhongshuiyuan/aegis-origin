﻿/// <copyright file="GeoJsonConvert.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2022 Roberto Giachetta. Licensed under the
///     Educational Community License, Version 2.0 (the "License"); you may
///     not use this file except in compliance with the License. You may
///     obtain a copy of the License at
///     http://opensource.org/licenses/ECL-2.0
///
///     Unless required by applicable law or agreed to in writing,
///     software distributed under the License is distributed on an "AS IS"
///     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
///     or implied. See the License for the specific language governing
///     permissions and limitations under the License.
/// </copyright>
/// <author>Norbert Vass</author>

using ELTE.AEGIS.IO.JSON;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace ELTE.AEGIS.IO.GeoJSON
{
    /// <summary>
    /// Represents a converter to GeoJsonObject.
    /// </summary>
    public class GeoJsonConvert : JsonCreationConverter<GeoJsonObject>
    {
        protected override GeoJsonObject Create(Type objectType, JObject jObject)
        {
            if (jObject["type"] != null)
            {
                if (jObject["type"].ToString() == "FeatureCollection" && jObject["features"] != null)
                {
                    return new FeatureCollection();
                }
                else if (jObject["type"].ToString() == "Feature")
                {
                    return new Feature();
                }
                else if (jObject["type"].ToString() == "Point" || jObject["type"].ToString() == "MultiPoint" ||
                         jObject["type"].ToString() == "LineString" ||
                         jObject["type"].ToString() == "MultiLineString" ||
                         jObject["type"].ToString() == "Polygon" || jObject["type"].ToString() == "MultiPolygon" ||
                         jObject["type"].ToString() == "GeometryCollection")
                {
                    return new GeometryObject();
                }
                else throw new IOException("Object type not supported.");
            }
            else throw new IOException("Wrong GeoJSON file format.");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
