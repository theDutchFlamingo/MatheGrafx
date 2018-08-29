'use strict';

var graph = angular.module('GraphClass', []);
var app = angular.module('GraphApp')

var canvas, ctx,
    functions,
    gridSpacing,
    mouseDown;

var x0, y0, x1, y1;
var centerX, centerY;

var width, height;

function init() {
    console.log("Hello world!");
    var txt = document.getElementById('txt');

    var ctrl = app.ctrl;
    txt.value = toString(ctrl);

    var canvas = document.getElementById('can');
    ctx = canvas.getContext('2d');
    width = canvas.width;
    height = canvas.height;
    functions = ctrl.Functions;
    gridSpacing = ctrl.gridSpacing;

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

    functions = functions.forEach(func => {
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