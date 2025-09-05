using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MapViewer.Config
{
	public class MapConfig
	{
		public List<LayerBase> Layers { get; set; } = [];
	}

	[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
	[JsonDerivedType(typeof(WmsLayer), "WMS")]
	[JsonDerivedType(typeof(WmtsLayer), "WMTS")]
	[JsonDerivedType(typeof(XyzLayer), "XYZ")]
	[JsonDerivedType(typeof(TmsLayer), "TMS")]

	public abstract class LayerBase
	{
		public string? Name { get; set; }
		public double? Opacity { get; set; }
		public bool? IsVisible { get; set; }
	}

	// WMS-specific
	public class WmsLayer : LayerBase
	{
		public string ServiceUri { get; set; } = string.Empty;
		public string? Layers { get; set; }
	}

	// WMTS-specific
	public class WmtsLayer : LayerBase
	{
		public string CapabilitiesUri { get; set; } = string.Empty;
		public string? Layer { get; set; }
		public string? TileMatrixSet { get; set; }
		public string? Format { get; set; }
	}

	// XYZ tile server
	public class XyzLayer : LayerBase
	{
		public string UriTemplate { get; set; } = string.Empty; // e.g. https://{s}.../{z}/{x}/{y}.png
		public string[]? Subdomains { get; set; } // e.g.  ["mt0", "mt1", "mt2", "mt3"]
	}

	// Tile map server
	public class TmsLayer : LayerBase
	{
		public string UriTemplate { get; set; } = string.Empty; // e.g. https://.../{z}/{x}/{y}.png
		public string[]? Subdomains { get; set; } // e.g. ["mt0", "mt1", "mt2", "mt3"]
	}
}
