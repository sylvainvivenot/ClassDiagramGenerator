var app = angular.module('classDiagramGeneratorApp');


app.controller('diagramGeneratorController',
    function diagramController($rootScope, $scope, $http) {
        $scope.disabled = false;
        function wait() {
            document.body.style.cursor = 'wait';
            $scope.disabled = true;
        }

        function backToDefault() {
            document.body.style.cursor = 'default';
            $scope.disabled = false;
        }

         

        $scope.setFile = function (element) {
            var file = element.files[0];
            $scope.fileToUpload = file;
            $scope.name = file.name;
            $scope.size = file.size;
            $scope.$apply();
        }

        $scope.buttonName = "Send";

        function generateDiagram(filePath, callback) {
            var diagram = null;
            var solution = {
                ArchivePath: filePath,
                SlnFile: ""
            };
            $http.post('/Api/ClassDiagram/', JSON.stringify(solution)).then(
                function (response) {
                    console.log(response);
                    diagram = response.data;
                    console.log("diagram : " + diagram);
                    callback(diagram);
                });

        }

        function uploadFile() {
            wait();
            var reqObj = new XMLHttpRequest();

            reqObj.upload.addEventListener("progress", uploadProgress, false);
            reqObj.addEventListener("load", uploadComplete, false);


            reqObj.open("POST", "api/UploadFile", true);
            reqObj.setRequestHeader("Content-Type", "multipart/form-data");

            reqObj.setRequestHeader('X-File-Name', $scope.name);
            reqObj.setRequestHeader('X-File-Type', $scope.type);
            reqObj.setRequestHeader('X-File-Size', $scope.size);
            reqObj.send($scope.fileToUpload);


            function uploadProgress(evt) {
                if (evt.lengthComputable) {
                    var uploadProgressCount = Math.round(evt.loaded * 100 / evt.total);
                    console.log(evt.loaded);
                    $scope.loaded = uploadProgressCount;
                    $scope.$apply();
                }
            }

            reqObj.addEventListener("load", uploadComplete, false);

            function uploadComplete(evt) {
                /* This event is raised when the server  back a response */
                console.log("response " + reqObj.response);
                var filePath = JSON.parse(reqObj.responseText).filePath;
                generateDiagram(filePath, function (diagram) {
                    console.log("diagram : " + diagram);
                    $scope.$broadcast("diagram", { diagram: diagram });
                    backToDefault();
                });
                $scope.loaded = 100;
                $scope.$apply();
                
            }
        }


        $scope.SendData = function () {
            console.log("file: " + $scope.file);
            uploadFile();
        }
    });