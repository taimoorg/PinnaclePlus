<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="map_view.aspx.vb" Inherits=".map_view" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <style>
       #dvMap {
        height: 800px;
        width: 100%;
       }
    </style>
    <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?key=AIzaSyDM7-igdY9_vzj7-1ASLXQmLHPxNDB3OHE&libraries=drawing"></script>
    
</head>
<body>
    <form id="form1" runat="server">

    <script type="text/javascript">
        var markers = [<asp:Repeater ID="rptMarkers" runat="server"><ItemTemplate>{"title": '<%# Eval("Name") %>',
        "lat": <%# Eval("Latitude") %>,
        "lng": <%# Eval("Longitude") %>,
        "OrderNo": '<%# Eval("OrderNo")%>',
        "icon": 'icon/3.png'}</ItemTemplate><SeparatorTemplate>,</SeparatorTemplate></asp:Repeater>];
        var selMarkers=[];
        var allMarkers=[];
        var shape=null;
    </script>


<script type="text/javascript">
    function DisplayList()
    {
        var html
        html=""
        for (i = 0; i<selMarkers.length;i++)
        {
            html=html + selMarkers[i].getTitle() +"</br>";
        }
        sel.innerHTML=html;
    }
    
    window.onload = function () {
        var drawingManager = new google.maps.drawing.DrawingManager();
        var uluru = { lat: markers[0].lat, lng: markers[0].lng};
        var map = new google.maps.Map(document.getElementById('dvMap'), {
            zoom:6,
            center: uluru
        });

        drawingManager.setOptions({
            //drawingMode : google.maps.drawing.OverlayType.RECTANGLE,
            drawingControl : true,
            drawingControlOptions : {
                position : google.maps.ControlPosition.TOP_CENTER,
                drawingModes : [ google.maps.drawing.OverlayType.RECTANGLE,google.maps.drawing.OverlayType.CIRCLE ]
            },
            rectangleOptions : {
                strokeColor : '#a3ccff',
                strokeWeight : 1,
                fillColor : '#b8b8b8',
                fillOpacity : 0.6
                //,
                //editable: true,
                //draggable: true
            }	
        });
        // Loading the drawing Tool in the Map.
        drawingManager.setMap(map);

        google.maps.event.addListener(drawingManager, 'overlaycomplete', function(e) {
            //if (shape==null)
            //{
            //    shape=e;
            //}
            //else
            //{
            //    shape.overlay.setMap(null);
            //    shape=e;
            //}

            var bounds = new google.maps.LatLngBounds(e.overlay.getBounds().getSouthWest(), e.overlay.getBounds().getNorthEast());
            for (a=0;a<allMarkers.length;a++) {
                if (bounds.contains(new google.maps.LatLng(allMarkers[a].position.lat(), allMarkers[a].position.lng()))) {
                    allMarkers[a].setIcon("/icon/111.png");
                    selMarkers.push(allMarkers[a]);
                    DisplayList();
                }
            }
            e.overlay.setMap(null);
        });

        var infoWindow = new google.maps.InfoWindow();
        
        for (i = 0; i < markers.length; i++) {
            var data = markers[i]
            var myLatlng = new google.maps.LatLng(data.lat, data.lng);
            var marker = new google.maps.Marker({
                position: myLatlng,
                map: map,
                title: data.title,
                M_ID:data.OrderNo,
                Icon:data.icon
            });
            allMarkers.push(marker);
            (function (marker, data) {
                
                google.maps.event.addListener(marker, "rightclick", function (e) {
                    infoWindow.setContent(data.OrderNo);
                    infoWindow.open(map, marker);
                });
                google.maps.event.addListener(marker, "click", function (e) {
                    //infoWindow.setContent(data.description);
                    //infoWindow.open(map, marker);
                    var found
                    found=false;
                    for (i = 0; i<selMarkers.length;i++)
                    {
                        if (marker.M_ID==selMarkers[i].M_ID)
                        {
                            marker.setIcon("/icon/3.png");
                            selMarkers.splice(i,1);
                            found=true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        marker.setIcon("/icon/111.png");
                        selMarkers.push(marker);
                    }
                    
                    DisplayList();
                });
            })(marker, data);
        }
    }

    

</script>
<div id="dvMap" >
    </div>
        <div id="sel"></div>
    </form>
</body>
</html>
