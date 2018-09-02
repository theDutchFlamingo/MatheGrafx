'use strict';

var app = angular.module('GraphApp', []);
var ctrl;
var canvas = document.getElementById('graph');
var ctx = canvas.getContext('2d');

var mouseDown;

var x0, y0, x1, y1;
var centerX, centerY;
var width, height;

document.addEventListener('DOMContentLoaded', function () {
    angular.bootstrap(document, ['GraphApp']);
});

app.controller('GraphController', function ($scope, GraphService) {
    ctrl = this;
    $scope.Title = 'Graph List';
    $scope.Init = init;
    $scope.Draw = draw;

    LoadGraph();
    init();

    function LoadGraph() {
        GraphService.Get()
            .then(function (graph) {
                ctrl.Graph = graph;
                $scope.Graph = graph;
            }, function (error) {
                ctrl.ErrorMessage = error
            });
    }
});

app.service('GraphService', function ($http) {
    var svc = this;
    var apiUrl = 'http://localhost:5000/api';

    svc.ctrl = ctrl;

    svc.Get = function () {
        return $http.get(apiUrl + '/graph')
            .then(function success(response) {
                return response.data;
            });
    }

    svc.Post = function () {
        return $http.post(apiUrl + '/graph')
            .then(function success(response) {
                return response.data;
            });
    }
});

function init() {
    console.log("Hello world!");

    console.log(ctrl);

    var graph = ctrl.Graph;

    width = canvas.width;
    height = canvas.height;
    functions = graph.Functions;
    gridSpacing = graph.gridSpacing;

    canvas.addEventListener("mousemove", function(e) {
        move("move", e)
    }, false);

    canvas.addEventListener("mousedown", function(e) {
        move("down", e)
    }, false);

    canvas.addEventListener("mouseup", function(e) {
        move("up", e)
    }, false);

    canvas.addEventListener("mouseout", function(e) {
        move("out", e)
    }, false);

    draw();
}

function draw() {
    var prevX, prevY;

    functions.forEach(func => {
        for (var i = 0; i < func.Points.length; i++) {
            ctx.beginPath();
            ctx.moveTo(prevX, prevY);
            ctx.lineTo(func.Points[i].X, func.Points[i]);
            prevX = func.Points[i].X;
            prevY = func.Points[i].Y;
            ctx.strokeStyle = color;
            ctx.lineWidth = stroke;
            ctx.stroke();
            ctx.closePath();
        }
    });
}

function move(str, e) {
    switch (str) {
        case "move":
            if (!mouseDown) {

            }
            break;
        case "out":
        case "up":
            mouseDown = false;
            break;
        case mouseDown:
            x0 = x1;
            y0 = y1;
            x1 = e.clientX - canvas.offsetLeft;
            y1 = e.clientY - canvas.offsetTop;
            draw();
    }
}