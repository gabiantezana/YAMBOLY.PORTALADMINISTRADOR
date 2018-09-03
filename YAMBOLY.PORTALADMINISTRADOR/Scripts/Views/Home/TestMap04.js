const drawingModes = Object.freeze({ drawZoneMode: 1, drawRouteMode: 2 });
var colors = ['red', 'green', 'blue', 'orange', 'yellow', 'BlueViolet ', 'DarkMagenta', 'Brown ', 'Violet', 'Lime', 'GoldenRod', 'CadetBlue'];
var latLngCenterInit = new google.maps.LatLng(-12.0912651, -77.00467609999998);

var Shape = function () { };
Shape.prototype.id = null;
Shape.prototype.type = null;
Shape.prototype.polygon = null;
Shape.prototype.parentPolygonId = null;

var polygonOptionsZone = {
    strokeWeight: 0.5,
    fillOpacity: 0.40,
    editable: true,
    //fillColor: '#ffff00'
    fillColor: 'HotPink'
}

var polygonOptionsRoute = {
    strokeWeight: 0,
    fillOpacity: 0.40,
    editable: true,
    fillColor: '#FF1493'
}

function isPolygonInsidePolygon(innerPolygon, outerPolygon) {
    debugger;
    var pointsInside = 0;
    var pointsOutside = 0;
    innerPolygon.getPath().getArray().map(function (x) {
        (google.maps.geometry.poly.containsLocation(x, outerPolygon)) ? pointsInside++ : pointsOutside++;
    });
    return (pointsOutside > 0) ? false : true;
};





function GetPointsFromPolygon(polygon) {
    var coordinates = [];
    var message = '';
    message += "polygon points:" + "<br>";
    for (var i = 0; i < polygon.getPath().getLength(); i++) {
        coordinates.push({
            lat: polygon.getPath().getAt(i).lat(),
            lng: polygon.getPath().getAt(i).lng()
        });
        //message += polygon.getPath().getAt(i).toUrlValue(6) + "<br>";
    }

    console.log(coordinates);
    return coordinates;
}

function GetZonePolygoneFromRoutePolygon(routePolygon) {
    return _.findWhere(polygonArray, { parentPolygonId: routePolygon.parentPolygonId });
}

function RemoveSelectedPolygon() {
    if (selectedShape)
        selectedShape.setMap(null);
    drawingManager.setMap(null);

}

function clearSelection() {
    if (selectedShape) {
        selectedShape.setEditable(false);
        selectedShape = null;
    }
}

function setSelection(shape) {
    clearSelection();
    selectedShape = shape;
    shape.setEditable(true);//TODO:
}


function OnCreateClick(mode) {
    switch (currentSelectedItem.ShapeType.toString()) {
        case '1':
        case '2':
            SetDrawingManagerOptions(drawingManagerPolygonOptions);
            break;
        case '3':
            SetDrawingManagerOptions(drawingManagerMarkerOptions);
            break;
        default:
            break;
    }
    //drawingMode = mode;
    //clearSelection();
    drawingManager.setMap(map);
    drawingManager.setDrawingMode(google.maps.drawing.OverlayType.POLYGON);
    switch (mode) {
        case drawingModes.drawRouteMode:
            drawingManager.set('polygonOptions', polygonOptionsRoute);
            break;
        case drawingModes.drawZoneMode:
            drawingManager.set('polygonOptions', polygonOptionsZone);
            break;
        default:
            break;
    }
}

function UpdatePolygon(polygon) {
    //Mode 1:
    /*polygonArray = _.without(polygonArray, _.findWhere(polygonArray, { id: polygon.id }))
    polygonArray.push(polygon);*/

    //Mode 2:
    var match = _.find(polygonArray, function (item) { return item.id === polygon.id })
    if (match)
        match = polygon;
}

