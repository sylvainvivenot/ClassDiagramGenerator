var app = angular.module('classDiagramGeneratorApp');
app.controller('diagramShowerController',
    function diagramController($rootScope, $scope, $http) {
        $scope.$on("diagram",
            function (event, args) {
                var diagram = args.diagram;
                console.log("show : " +diagram);
                init(diagram.classes, diagram.links);
            });

    });