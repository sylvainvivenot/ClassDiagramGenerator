var app = angular.module('classDiagramGeneratorApp', ['file-model']);
app.controller('diagramController',
    function diagramController($scope, $http) {
        $scope.buttonName = "Send";
        
        function generateDiagram(filePath) {
           
            $http.get('/Api/ClassDiagram/'+filePath).then(
                function(response) {
                    console.log(response);
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
                    generateDiagram(filePath);
                    $scope.response = filePath;
                },function(response) {
                    $scope.response = response;
                });
        }
    });