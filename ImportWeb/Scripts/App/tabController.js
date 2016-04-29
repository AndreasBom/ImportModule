(function() {
    app.controller('TabController', function($scope) {
        $scope.tabs = [
            {
                title: 'Meny 1',
                url: '1'
            }, {
                title: 'Meny 2',
                url: '/test.html'
            }, {
                title: 'Meny 3',
                url: '3'
            }
        ];

        $scope.currentTab = '1';

        $scope.onClickTab = function (tab) {
            $scope.currentTab = tab.url;
        }
    
        $scope.isActiveTab = function (tabUrl) {
            return tabUrl === $scope.currentTab;
        }
    });
})();