using MapControl;
using MapViewer.Config;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace MapViewer
{
	public sealed partial class LayerCollectionControl : UserControl
	{
		public string HeaderText
		{
			get => HeaderTextBlock.Text;
			set => HeaderTextBlock.Text = value;
		}

		public required Map Map { get; set; }

		private MapConfig _config;

		public LayerCollectionControl(MapConfig config)
		{
			InitializeComponent();
			
			_config = config;
		}

		private void AddLayers(MapConfig config)
		{
			try
			{
				foreach (LayerBase layer in config.Layers)
				{
					LayerSettingsControl control;

					switch (layer)
					{
						case WmsLayer wmsLayer:
							WmsImageLayer wmsImageLayer;

							if (wmsLayer.ServiceUri.Contains("version=1.1.1", StringComparison.CurrentCultureIgnoreCase))
								wmsImageLayer = new LegacyWmsImageLayer();
							else
								wmsImageLayer = new();

							wmsImageLayer.ServiceUri = new Uri(wmsLayer.ServiceUri);
							wmsImageLayer.RequestLayers = wmsLayer.Layers;

							control = new(wmsImageLayer, Map)
							{
								LayerName = layer.Name ?? string.Empty,
								IsLayerEnabled = layer.IsVisible ?? false,
								LayerOpacity = layer.Opacity ?? 100.0,
							};

							LayersStackPanel.Children.Add(control);
							break;

						case WmtsLayer wmtsLayer:
							WmtsTileLayer wmtsTileLayer = new()
							{
								CapabilitiesUri = new Uri(wmtsLayer.CapabilitiesUri),
								SourceName = layer.Name,
							};

							control = new(wmtsTileLayer, Map)
							{
								LayerName = layer.Name ?? string.Empty,
								IsLayerEnabled = layer.IsVisible ?? false,
								LayerOpacity = layer.Opacity ?? 100.0,
							};

							LayersStackPanel.Children.Add(control);
							break;

						case XyzLayer xyzLayer:
							MapTileLayer mapTileLayer = new()
							{
								TileSource = new TileSource() { UriTemplate = xyzLayer.UriTemplate },
								SourceName = layer.Name,
							};

							if (xyzLayer.Subdomains != null && xyzLayer.Subdomains.Length > 0)
							{
								mapTileLayer.TileSource.Subdomains = xyzLayer.Subdomains;
							}

							control = new(mapTileLayer, Map)
							{
								LayerName = layer.Name ?? string.Empty,
								IsLayerEnabled = layer.IsVisible ?? false,
								LayerOpacity = layer.Opacity ?? 100.0,
							};
							LayersStackPanel.Children.Add(control);
							break;
						case TmsLayer tmsLayer:
							MapTileLayer tileMapLayer = new()
							{
								TileSource = new TmsTileSource() { UriTemplate = tmsLayer.UriTemplate },
								SourceName = layer.Name,
							};

							if (tmsLayer.Subdomains != null && tmsLayer.Subdomains.Length > 0)
							{
								tileMapLayer.TileSource.Subdomains = tmsLayer.Subdomains;
							}

							control = new(tileMapLayer, Map)
							{
								LayerName = layer.Name ?? string.Empty,
								IsLayerEnabled = layer.IsVisible ?? false,
								LayerOpacity = layer.Opacity ?? 100.0,
							};
							LayersStackPanel.Children.Add(control);
							break;
					}
				}
			}
			catch (Exception ex)
			{
				var dialog = new ContentDialog
				{
					Title = "Error",
					Content = ex.Message,
					CloseButtonText = "OK",
					XamlRoot = this.XamlRoot
				};

				_ = dialog.ShowAsync();
			}
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			AddLayers(_config);
		}

		private void CloseButton_Click(object sender, RoutedEventArgs e)
		{
			if (this.Parent is Panel parentPanel)
			{
				parentPanel.Children.Remove(this);
			}
		}
	}
}
