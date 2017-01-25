var app = angular.module('classDiagramGeneratorApp');
app.controller('diagramGeneratorController',
    function diagramController($rootScope, $scope, $http) {
        $scope.buttonName = "Send";
        
        function generateDiagram(filePath,callback) {
            var diagram = null;
            $http.get('/Api/ClassDiagram/'+filePath).then(
                function(response) {
                    console.log(response);
                    diagram = response.data;
                    console.log("diagram : " + diagram);
                    callback(diagram);
                });
            
        }

        
        $scope.SendData = function () {
            console.log("file: " + $scope.file);

            var fd = new FormData();
            fd.append('file', $scope.file);

            $http.post('/Api/UpLoadFile', fd,
                {
                    withCredentials: false,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity,
                    params: { fd }
                    //responseType: "arraybuffer"
                })
                .then(function(response) {
                    var filePath = response.data.filePath;
                    generateDiagram(filePath,function(diagram) {
                        console.log("diagram : " + diagram);
                        $scope.$broadcast("diagram", { diagram: diagram });
                    });

                    $rootScope.response = filePath;
                },function(response) {
                    $rootScope.response = response;
                });
        }
    });