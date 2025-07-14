using MapControl;
using MapViewer.Config;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
using System.Text.Json;
using WinRT.Interop;

namespace MapViewer
{
	/// <summary>
	/// An empty window that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			var hwnd = WindowNative.GetWindowHandle(this);
			var windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
			var appWindow = AppWindow.GetFromWindowId(windowId);
			appWindow.SetIcon("icon.ico");

			AddLayers(GetSampleConfig());

			// Uncomment to get a sample config in the clipboard
			//SerializeConfig(GetSampleConfig());
		}

		private static MapConfig GetSampleConfig()
		{
			MapConfig config = new();

			config.Layers.Add(new XyzLayer
			{
				Name = "Topografisk (Lantmäteriet)",
				UriTemplate = @"https://minkarta.lantmateriet.se/map/topowebbcache?layer=topowebb&tilematrixset=3857&Service=WMTS&Request=GetTile&TileMatrix={z}&TileCol={x}&TileRow={y}",
				//UriTemplate = @"https://karta.raa.se/lmtopowebb/1.0.0/topowebb/default/3857/{z}/{y}/{x}.png",
				Opacity = 100.0,
				IsVisible = true
			});

			config.Layers.Add(new TmsLayer
			{
				Name = "Topografisk (Hitta.se)",
				UriTemplate = @"https://static.hitta.se/tile/v3/4/{z}/{x}/{y}?v=27032025",
				Opacity = 100.0,
				IsVisible = false
			});

			config.Layers.Add(new XyzLayer
			{
				Name = "Terräng (Google)",
				UriTemplate = @"http://mt1.google.com/vt/lyrs=p&x={x}&y={y}&z={z}",
				Opacity = 100.0,
				IsVisible = false
			});

			config.Layers.Add(new WmsLayer
			{
				Name = "Nedtonad",
				ServiceUri = "https://minkarta.lantmateriet.se/map/topowebb",
				Layers = "topowebbkartan_nedtonad",
				Opacity = 100.0,
				IsVisible = false,
			});

			config.Layers.Add(new TmsLayer
			{
				Name = "Vägkarta (Hitta.se)",
				UriTemplate = @"https://static.hitta.se/tile/v3/0/{z}/{x}/{y}?v=27032025",
				Opacity = 100.0,
				IsVisible = false
			});

			config.Layers.Add(new TmsLayer
			{
				Name = "Vägkarta (Eniro)",
				UriTemplate = @"http://map.eniro.com/geowebcache/service/tms1.0.0/map/{z}/{x}/{y}.png",
				Opacity = 100.0,
				IsVisible = false
			});
			config.Layers.Add(new XyzLayer
			{
				Name = "Vägkarta (Google)",
				UriTemplate = @"http://mt0.google.com/vt/x={x}&y={y}&z={z}",
				Opacity = 100.0,
				IsVisible = false
			});

			config.Layers.Add(new XyzLayer
			{
				Name = "Vägkarta (OpenStreetMap)",
				UriTemplate = @"https://tile.openstreetmap.org/{z}/{x}/{y}.png",
				Opacity = 100.0,
				IsVisible = false
			});

			/*WmtsLayer karta = new()
			{
				Name = "Topografisk karta",
				CapabilitiesUri = @"https://minkarta.lantmateriet.se/map/topowebbcache?&Service=WMTS&Request=GetCapabilities",
				Layer = "topowebb",
				TileMatrixSet = "3857",
				Opacity = 100.0,
				IsVisible = false
			};
			config.Layers.Add(karta);*/

			config.Layers.Add(new WmsLayer
			{
				Name = "Flygbild (Lantmäteriet)",
				ServiceUri = "https://minkarta.lantmateriet.se/map/ortofoto",
				Layers = "Ortofoto_0.5,Ortofoto_0.4,Ortofoto_0.25,Ortofoto_0.16",
				Opacity = 100.0,
				IsVisible = false,
			});

			config.Layers.Add(new TmsLayer
			{
				Name = "Flygbild (Eniro)",
				UriTemplate = @"http://map.eniro.com/geowebcache/service/tms1.0.0/aerial/{z}/{x}/{y}.jpeg",
				Opacity = 100.0,
				IsVisible = false
			});

			config.Layers.Add(new TmsLayer
			{
				Name = "Flygbild (Hitta.se)",
				UriTemplate = @"https://static.hitta.se/tile/v3/1/{z}/{x}/{y}?v=27032025",
				Opacity = 100.0,
				IsVisible = false
			});

			config.Layers.Add(new XyzLayer
			{
				Name = "Flygbild (Google)",
				UriTemplate = @"http://khm1.google.com/kh/v=101&x={x}&y={y}&z={z}",
				Opacity = 100.0,
				IsVisible = false
			});

			config.Layers.Add(new WmsLayer
			{
				Name = "Flygbild ca 1960",
				ServiceUri = "https://minkarta.lantmateriet.se/map/historiskaortofoto",
				Layers = "OI.Histortho_60",
				Opacity = 100.0,
				IsVisible = false,
			});

			config.Layers.Add(new WmsLayer
			{
				Name = "Flygbild ca 1975",
				ServiceUri = "https://minkarta.lantmateriet.se/map/historiskaortofoto",
				Layers = "OI.Histortho_75",
				Opacity = 100.0,
				IsVisible = false,
			});

			config.Layers.Add(new WmsLayer
			{
				Name = "Sjökort (Lantmäteriet)",
				ServiceUri = "https://karta.raa.se/sjokort",
				Layers = "OgcWmsLayer0",
				Opacity = 100.0,
				IsVisible = false,
			});

			config.Layers.Add(new TmsLayer
			{
				Name = "Sjökort (Eniro)",
				UriTemplate = @"http://map.eniro.com/geowebcache/service/tms1.0.0/nautical/{z}/{x}/{y}.jpeg",
				Opacity = 100.0,
				IsVisible = false
			});

			config.Layers.Add(new WmsLayer
			{
				Name = "Lutning",
				ServiceUri = "https://karta.raa.se/lmhojdmodell",
				Layers = "terranglutning",
				Opacity = 100.0,
				IsVisible = false,
			});

			config.Layers.Add(new WmsLayer
			{
				Name = "Skuggning",
				ServiceUri = "https://karta.raa.se/lmhojdmodell",
				Layers = "terrangskuggning",
				Opacity = 100.0,
				IsVisible = false,
			});

			config.Layers.Add(new WmsLayer
			{
				Name = "Fastighetsgränser",
				ServiceUri = "https://karta.raa.se/lmfastighet?TRANSPARENT=true",
				Layers = "granser,text",
				Opacity = 100.0,
				IsVisible = false,
			});

			config.Layers.Add(new WmsLayer
			{
				Name = "Sockengränser",
				ServiceUri = "https://karta.raa.se/geo/arkreg_v1.0/wms?TRANSPARENT=true",
				Layers = "socken",
				Opacity = 100.0,
				IsVisible = false,
			});

			config.Layers.Add(new WmsLayer
			{
				Name = "Landskapsgränser",
				ServiceUri = "https://karta.raa.se/geo/arkreg_v1.0/wms?TRANSPARENT=true",
				Layers = "landskap",
				Opacity = 100.0,
				IsVisible = false,
			});

			config.Layers.Add(new WmsLayer
			{
				Name = "Lämningar",
				ServiceUri = "https://karta.raa.se/geo/arkreg_v1.0/wms?TRANSPARENT=true",
				Layers = "arkreg_v1.0:publicerade_lamningar_centrumpunkt",
				Opacity = 100.0,
				IsVisible = false,
			});

			config.Layers.Add(new WmsLayer
			{
				Name = "Befolkningstäthet",
				ServiceUri = "https://minkarta.lantmateriet.se/map/scb?TRANSPARENT=true",
				Layers = "PD.StatisticalDistribution.TotalPopulation",
				Opacity = 50.0,
				IsVisible = false,
			});

			config.Layers.Add(new XyzLayer
			{
				Name = "Velinga 1704 (XYZ)",
				UriTemplate = @"https://wmts.oldmapsonline.org/maps/c3ac2835-389f-413b-a807-7945ee0c8584/2025-06-30T13:58:01.258Z/{z}/{x}/{y}.png?key=8mpRUId0ULH39v2JucO5",
				Opacity = 100.0,
				IsVisible = false
			});

			config.Layers.Add(new WmtsLayer()
			{
				Name = "Velinga 1704 (WMTS)",
				CapabilitiesUri = @"https://wmts.oldmapsonline.org/maps/c3ac2835-389f-413b-a807-7945ee0c8584/2025-06-30T13:58:01.258Z/WMTSCapabilities.xml?key=8mpRUId0ULH39v2JucO5",
				Opacity = 100.0,
				IsVisible = false
			});
			


			return config;
		}

		private string SerializeConfig(MapConfig config)
		{
			return JsonSerializer.Serialize(config);
		}

		private void AddLayers(MapConfig config)
		{
			foreach (LayerBase layer in config.Layers)
			{
				LayerSettingsControl control;

				switch (layer)
				{
					case WmsLayer wmsLayer:
						WmsImageLayer wmsImageLayer = new()
						{
							ServiceUri = new Uri(wmsLayer.ServiceUri),
							WmsLayers = wmsLayer.Layers
						};
						
						control = new(wmsImageLayer, Map)
						{
							LayerName = layer.Name,
							IsLayerEnabled = layer.IsVisible,
							LayerOpacity = layer.Opacity,
						};

						LayersStackPanel.Children.Add(control);
						break;

					case WmtsLayer wmtsLayer:
						WmtsTileLayer wmtsTileLayer = new()
						{
							CapabilitiesUri = new Uri(wmtsLayer.CapabilitiesUri),
							SourceName = layer.Name
						};
						
						control = new(wmtsTileLayer, Map)
						{
							LayerName = layer.Name,
							IsLayerEnabled = layer.IsVisible,
							LayerOpacity = layer.Opacity,
						};

						LayersStackPanel.Children.Add(control);
						break;

					case XyzLayer xyzLayer:
						MapTileLayer mapTileLayer = new()
						{
							TileSource = new TileSource() { UriTemplate = xyzLayer.UriTemplate },
							SourceName = layer.Name,
						};
						
						control = new(mapTileLayer, Map)
						{
							LayerName = layer.Name,
							IsLayerEnabled = layer.IsVisible,
							LayerOpacity = layer.Opacity,
						};
						LayersStackPanel.Children.Add(control);
						break;
					case TmsLayer tmsLayer:
						MapTileLayer tileMapLayer = new()
						{
							TileSource = new TmsTileSource() { UriTemplate = tmsLayer.UriTemplate },
							SourceName = layer.Name,
						};

						control = new(tileMapLayer, Map)
						{
							LayerName = layer.Name,
							IsLayerEnabled = layer.IsVisible,
							LayerOpacity = layer.Opacity,
						};
						LayersStackPanel.Children.Add(control);
						break;

				}
			}
		}
	}
}
