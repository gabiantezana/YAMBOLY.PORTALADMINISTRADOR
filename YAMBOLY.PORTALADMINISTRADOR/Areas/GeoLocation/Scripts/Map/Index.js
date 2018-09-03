//const drawingModes = Object.freeze({ drawZoneMode: 1, drawRouteMode: 2 });
const BUTTONMODE = Object.freeze({ Create: 1, Edit: 2, Remove: 3 });
const SHAPETYPE = Object.freeze({ Zone: 1, Route: 2, Address: 3 });
const mouseOverColor = "#00FF00";

var drawingManagerPolygonOptions = {
    drawingMode: google.maps.drawing.OverlayType.POLYGON,
    drawingControl: false,
    markerOptions: {
        draggable: true
    }
};

var drawingManagerMarkerOptions = {
    drawingMode: google.maps.drawing.OverlayType.MARKER,
    drawingControl: false,
    markerOptions: {
        draggable: true
    }
};

var polygonOptionsZone = {
    strokeWeight: 0.7,
    fillOpacity: 0.0,
    editable: true,
    fillColor: 'yellow',
};

var polygonOptionsRoute = {
    strokeWeight: 0,
    fillOpacity: 0.40,
    editable: true,
    fillColor: '#FF1493',
};

function isPolygonInsidePolygon(innerPolygon, outerPolygon) {
    var pointsInside = 0;
    var pointsOutside = 0;
    innerPolygon.getPath().getArray().map(function (x) {
        (google.maps.geometry.poly.containsLocation(x, outerPolygon)) ? pointsInside++ : pointsOutside++;
    });
    return (pointsOutside > 0) ? false : true;
}

function _isPolygonIntersectedWithAnother(innerPolygon, outerPolygon, validateOverlaping) {
    debugger;
    var isIntersected = false;
    var isOverlaping = false;

    var pointsInside = 0;
    var pointsOutside = 0;
    innerPolygon.getPath().getArray().map(function (x) {
        (google.maps.geometry.poly.containsLocation(x, outerPolygon)) ? pointsInside++ : pointsOutside++;
    });
    //return (pointsInside > 0) ? true : false;
    if (pointsInside > 0)
        isIntersected = true;
    else
        isOverlaping = IsPolygonOverlaping(innerPolygon, outerPolygon);

    if (validateOverlaping) {
        if (isIntersected || isOverlaping)
            return true;
        else
            return false;
    }
    else {
        return isIntersected;
    }
}

function GetPointsFromPolygon(polygon) {
    var coordinates = [];
    var message = '';
    message += "polygon points:" + "<br>";
    for (var i = 0; i < polygon.getPath().getLength(); i++) {
        coordinates.push({
            lat: polygon.getPath().getAt(i).lat(),
            lng: polygon.getPath().getAt(i).lng()
        });
    }
    return coordinates;
}

function GetZonePolygoneFromRoutePolygon(routePolygon) {
    return _.findWhere(polygonArray, { parentPolygonId: routePolygon.parentPolygonId });
}

function RemoveSelectedPolygon() {
    if (currentSelectedShape) {
        currentSelectedShape.setMap(null);
        drawingManager.setMap(null);
        editedPolygonArray = _.without(editedPolygonArray, _.findWhere(editedPolygonArray, { Id: currentSelectedShape.Id, ShapeType: currentSelectedShape.ShapeType }));
    }
}

function clearSelection() {
    if (currentSelectedShape) {
        switch (currentSelectedShape.ShapeType) {
            case SHAPETYPE.Zone:
            case SHAPETYPE.Route:
                currentSelectedShape.setEditable(false);
                break;
            case SHAPETYPE.Address:
                currentSelectedShape.setDraggable(false);
                break;
            default:
                throw "Invalid shape type";
        }
        currentSelectedShape = null;
    }
}

function setSelection(shape) {//TODO:
    clearSelection();
    currentSelectedShape = shape;
    switch (shape.ShapeType) {
        case SHAPETYPE.Zone:
        case SHAPETYPE.Route:
            shape.setEditable(true);
            break;
        case SHAPETYPE.Address:
            shape.setDraggable(true);
            break;
        default:
            throw "Invalid shape type";
    }
}

function GetPolygonCenter(poly) {
    var lowx,
        highx,
        lowy,
        highy,
        lats = [],
        lngs = [],
        vertices = poly.getPath();

    for (var i = 0; i < vertices.length; i++) {
        lngs.push(vertices.getAt(i).lng());
        lats.push(vertices.getAt(i).lat());
    }

    lats.sort();
    lngs.sort();
    lowx = lats[0];
    highx = lats[vertices.length - 1];
    lowy = lngs[0];
    highy = lngs[vertices.length - 1];
    center_x = lowx + ((highx - lowx) / 2);
    center_y = lowy + ((highy - lowy) / 2);
    return (new google.maps.LatLng(center_x, center_y));
}

function IsAValidShape(shape) {
    var isValid = false;
    switch (shape.ShapeType) {
        case SHAPETYPE.Zone:
            isValid = IsAValidZone(shape);
            break;
        case SHAPETYPE.Route:
            isValid = ValidateCreatedRoute(shape);
            break;
        case SHAPETYPE.Address:
            var parentRoute = _.findWhere(editedPolygonArray, { Id: shape.ParentId, ShapeType: SHAPETYPE.Route });
            isValid = isMarkerInsidePolygon(shape, parentRoute);
            break;
        default:
            throw "Invalid shape type";
    }

    return isValid;

    /*/Change color for next draw in routes
    var polygonOptions = drawingManager.get('polygonOptions');
    polygonOptions.fillColor = colors[Math.floor(Math.random() * colors.length)];
    drawingManager.set('polygonOptions', polygonOptions);*/
}

function IsAValidZone(polygon) {
    //-----------------VALIDA QUE NO INTERSECTE A OTRAS ZONAS -------------------
    var isIntersected = false;
    for (var i = 0; i < editedPolygonArray.length; i++) {
        if (editedPolygonArray[i].Id !== polygon.Id) {
            if (editedPolygonArray[i].ShapeType === SHAPETYPE.Zone)
                isIntersected = _isPolygonIntersectedWithAnother(polygon, editedPolygonArray[i], true);
            else if (editedPolygonArray[i].ShapeType === SHAPETYPE.Route)
                isIntersected = _isPolygonIntersectedWithAnother(polygon, editedPolygonArray[i], false);
            if (isIntersected) {
                break;
            }
        }
    };
    return !isIntersected;
}

function ValidateCreatedRoute(polygon) {
    var isValid = false;
    //------------VALIDA QUE ESTÉ DIBUJADO DENTRO DE SU ZONA CORRESPONDIENTE---------
    parentZone = _.findWhere(editedPolygonArray, { Id: polygon.ParentId });
    if (parentZone)
        isValid = isPolygonInsidePolygon(polygon, parentZone);
    else
        console.log("No se encontró la zona del polígono: " + polygon.ParentId + " en el array.");

    //-----------------VALIDA QUE NO INTERSECTE A NINGUNA OTRA RUTA -------------------
    if (isValid) {
        for (var i = 0; i < editedPolygonArray.length; i++) {
            if (editedPolygonArray[i].ShapeType === SHAPETYPE.Route && editedPolygonArray[i].Id !== polygon.Id) {
                isValid = !_isPolygonIntersectedWithAnother(polygon, editedPolygonArray[i]);
                if (!isValid) {
                    break;
                }
            }
        };
    }
    return isValid;
}

function GetRandomColor() {
    var colors = ['red', 'green', 'blue', 'orange', 'yellow', 'BlueViolet ', 'DarkMagenta', 'Brown ', 'Violet', 'Lime', 'GoldenRod', 'CadetBlue'];
    var item = colors[Math.floor(Math.random() * colors.length)];
    return item;
}

google.maps.Polygon.prototype.getBounds = function () {
    var bounds = new google.maps.LatLngBounds();
    var paths = this.getPaths();
    var path;
    for (var i = 0; i < paths.getLength(); i++) {
        path = paths.getAt(i);
        for (var ii = 0; ii < path.getLength(); ii++) {
            bounds.extend(path.getAt(ii));
        }
    }
    return bounds;
};

function isMarkerInsidePolygon(marker, polygon) {
    var route = GetShapeFromMap(marker.ParentId, SHAPETYPE.Route);
    if (route) {
        return google.maps.geometry.poly.containsLocation(marker.position, polygon);
    }
    else
        return false;
}



