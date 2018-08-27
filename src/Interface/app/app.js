'use strict';

var app = angular.module('GraphApp', []);

var points;

document.addEventListener('DOMContentLoaded', function () {
    angular.bootstrap(document, ['GraphApp']);
});

app.controller('GraphController', function (GraphService) {
    var ctrl = this;
    ctrl.Title = 'Graph List';

    LoadGraph();

    function LoadGraph() {
        GraphService.Get()
            .then(function (graph) {
                ctrl.Graph = graph;
                points = graph;
            }, function (error) {
                ctrl.ErrorMessage = error
            });
    }
});

app.service('GraphService', function ($http) {
    var svc = this;
    var apiUrl = 'http://localhost:5000/api';

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