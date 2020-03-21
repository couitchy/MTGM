(function () { // Wrap in function to prevent accidental globals
    if (location.protocol != "data:") {
        $(window).bind('hashchange', function () {
            window.parent.handleChildIframeUrlChange(location.hash)
        });
    }

    String.prototype.fileExists = function() {
        filename = this.trim();

        var response = jQuery.ajax({
            url: filename,
            type: 'HEAD',
            async: false
        }).status;

        return (response != "200") ? false : true;
    }

    function Serie(data) {
        var self=this;
        self.Series = ko.observable(data.Series);
        self.Folder = ko.computed(function() {
            var currentSeries = "UNDEF";
            if (series[self.Series()] != null ) {
                currentSeries = series[self.Series()][0];
            }
            return currentSeries;
        }, self);

        self.SeriesNM_MtG = ko.observable(data.SeriesNM_MtG);
        self.SeriesNM_FR = ko.observable(data.SeriesNM_FR);
        self.Year = ko.computed(function() {
            var year = "?";
            if (series[self.Series()] != null ) {
                year = series[self.Series()][4];
            }
            return year;
        }, self);

        self.NbCards = ko.computed(function() {
            var nb = "?";
            if (series[self.Series()] != null ) {
                nb = series[self.Series()][3];
            }
            return nb;
        }, self);
    }

    function Card(data) {
        var self = this;
        self.MultiverseId = ko.observable(data.MultiverseId);
        self.EncNbr = ko.observable(data.EncNbr);
        self.Title = ko.observable(data.Title);
        self.TitleFR = ko.observable(data.TitleFR);
        self.SubTypeVF = ko.observable(data.SubTypeVF);
        self.TexteFR = ko.observable(data.TexteFR);
        self.TexteFRHTML = ko.computed(function () {
            var texte = self.TexteFR();
            texte = texte.replace(/\n/g,"<br>");
            var cost = new RegExp("![^!]{1,}!","igm");
            var costs = self.TexteFR().match(cost);
            if ( costs != null ){
                for(i=0;i<costs.length;i++){
                    onecost=costs[i].replace(/!|\//g,"");
                    texte=texte.replace(costs[i],'<img src="./img/mana/'+onecost.toUpperCase()+'.png">');
                }
            }
            return texte;
        }, self);
        self.Rarity = ko.observable(data.Rarity);
        self.ComparableRarity = ko.computed(function () {
            if (self.Rarity() == "C" ){
                return 10;
            } else if (self.Rarity() == "U" ){
                return 20;
            } else if (self.Rarity() == "R" ){
                return 30;
            } else if (self.Rarity() == "M" ){
                return 40;
            } else {
                return 0;
            }
        }, self);
        self.RarityIcon = ko.computed(function () {
            var letter = self.Rarity();
            return './img/rarity/'+ letter + '.gif';
        }, self);
        self.Series = ko.observable(data.Series);
        self.SeriesNM_MtG = ko.observable(data.SeriesNM_MtG);
        self.SeriesNM_FR = ko.observable(data.SeriesNM_FR);
        self.Items = ko.observable(data.Items);
        self.Cost = ko.observable(data.Cost);
        self.Color = ko.observable(data.Color);
        self.Price = ko.observable(data.Price);
        self.Mana = ko.dependentObservable(function () {
            if (self.Cost().length == 0) {
                return null;
            }
            if (!isNaN(self.Cost())) {
                var images = [];
                images.push({ icon: './img/mana/'+self.Cost()+'.png' });
                return images;
            }

            var workCost = self.Cost();

            // Manage dragon maze cost
            var dragonRegex = new RegExp(" \/\/ ", "igm");
            workCost = workCost.replace(dragonRegex,"-"); // replace dragon maze mana with empty string

            var images = [];
            var ravnicaImages = [];

            // Manage ravnica mana cost

            var regex = new RegExp("[(].[\/].[)]", "igm");
            var ravnicaMana = workCost.match(regex); // Search all mana with structure (m/n)
            if (ravnicaMana == null)
                ravnicaMana = [];
            for (j=0; j < ravnicaMana.length; j++) {
                var currentMana = ravnicaMana[j];
                ravnicaImages.push({ icon: './img/mana/'+currentMana.charAt(1)+currentMana.charAt(3)+'.png' })
            }
            var remainingCost = workCost.replace(regex,""); // replace ravnica mana with empty string
            for (i = 0; i < remainingCost.length ; i++) {
                if (remainingCost.charAt(i) != '')
                    images.push({ icon: './img/mana/'+remainingCost.charAt(i)+'.png' });
                }
            for (j=0; j < ravnicaImages.length; j++) {
                images.push(ravnicaImages[j]);
            }
            return images;
        });
        self.Label = ko.computed(function() {
            return self.TitleFR() + " - " + self.SeriesNM_FR();
        }, self);
        self.LocalImage = ko.observable();
        self.RemoteImage = ko.observable();
        self.Image = ko.computed(function() {
            var currentSeries = "UNDEF";
            if (series[self.Series()] != null ) {
                currentSeries = series[self.Series()][0];
            }
            var usedTitle = self.Title().replace(":", ""); // escape invalid characters
            // Manage dragon maze names
            var dragonRegex = new RegExp(" \/\/ ", "igm");
            usedTitle = usedTitle.replace(dragonRegex,""); // replace dragon maze mana with empty string
            usedTitle = usedTitle.replace("Æ", "Ae");
            // Set lands to first land in each edition
            if (usedTitle.toLowerCase() == "plains" || usedTitle.toLowerCase() == "forest" || usedTitle.toLowerCase() == "swamp" || usedTitle.toLowerCase() == "mountain" || usedTitle.toLowerCase() == "island")
                usedTitle += "1";

            self.LocalImage = './img/cards/'+ currentSeries + "/" + usedTitle+'.full.jpg';
            //self.RemoteImage = 'http://mtgimage.com/set/' + currentSeries + "/" + usedTitle+'.full.jpg';
            self.RemoteImage = 'http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=' + self.MultiverseId() +'&type=card';
            return self.LocalImage;
        }, self);
    }

    function ColorList(data) {
        var self = this;
        var sortedId = "";
        self.innerList = ko.observableArray(data);
        self.nbColoredCards = ko.computed(function () {
            return self.innerList().length;
        }, self);


        self.sort = function (data, event) {
            //Clear previous class on previous id sorted
            if ( sortedId != "" ) {
                document.getElementById(sortedId).setAttribute("class","");
                sortedId = $(event.target).prop("id");
            }
            id = $(event.target).prop("id");
            var order = document.getElementById(id).className ? (document.getElementById(id).className=='asc' ? 1 : -1) : 1;

            if (order == 1) {
                if ( id.indexOf("nameFr") == 0 ) {
                    self.sortByNameFR();
                } else if ( id.indexOf("nameEn") == 0 ) {
                    self.sortByNameEN();
                } else if ( id.indexOf("typeFr") == 0 ) {
                    self.sortByTypeFR();
                } else if ( id.indexOf("seriesFr") == 0 ) {
                    self.sortBySeriesFR();
                } else if ( id.indexOf("rarity") == 0 ) {
                    self.sortByRarity();
                }
            } else {
                self.innerList.reverse();
            }

            // set sort icon
            document.getElementById(id).className = order==1 ? 'desc' : 'asc';
        }

        // Manage sorting capabilities
        self.sortByNameFR = function () {
            self.innerList.sort(function(a, b) {
                return (a.TitleFR().localeCompare( b.TitleFR()) < 0) ? -1 : 1;
            });
        }

        self.sortByNameEN = function () {
            self.innerList.sort(function(a, b) {
                return (a.Title().localeCompare( b.Title()) < 0)  ? -1 : 1;
            });
        }

        self.sortByTypeFR = function () {
            self.innerList.sort(function(a, b) {
                if (a.SubTypeVF() != null && b.SubTypeVF() != null)
                    return (a.SubTypeVF().localeCompare( b.SubTypeVF()) < 0)? -1 : 1;
                else if (a.SubTypeVF() == null && b.SubTypeVF() != null)
                    // A is inferior (null)
                    return -1;
                else if (b.SubTypeVF() == null && a.SubTypeVF() != null)
                    // B is inferior (null)
                    return 1;
                else
                    // Both subtype are null - return alphabetic order
                    return (a.TitleFR().localeCompare( b.TitleFR()) < 0)? -1 : 1;
            });
        }

        self.sortBySeriesFR = function () {
            self.innerList.sort(function(a, b) {
                return (a.SeriesNM_FR().localeCompare( b.SeriesNM_FR()) < 0) ? -1 : 1;
            });
        }

        self.sortByRarity = function () {
            self.innerList.sort(function(a, b) {
                return (a.ComparableRarity() > b.ComparableRarity()) ? -1 : 1;
            });
        }

        // bind details window with selected card
        $('#details').attr('data-bind', 'with : selectedCard');

        // Manage pagination
        self.pageSize = ko.observable(10);
        self.pageIndex = ko.observable(0);

        self.pagedList = ko.dependentObservable(function () {
            var size = self.pageSize();
            var start = self.pageIndex() * size;
            return self.innerList().slice(start, start + size);
        });
        self.maxPageIndex = ko.dependentObservable(function () {
            return Math.ceil(self.innerList().length/self.pageSize())-1;
        });
        self.previousPage = function () {
            if (self.pageIndex() > 0) {
                self.pageIndex(self.pageIndex() - 1);
            }
        };
        self.nextPage = function () {
            if (self.pageIndex() < self.maxPageIndex()) {
                self.pageIndex(self.pageIndex() + 1);
            }
        };

        self.firstPage = function () {
            if (self.pageIndex() > 0) {
                self.pageIndex(0);
            }
        };
        self.lastPage = function () {
            if (self.pageIndex() < self.maxPageIndex()) {
                self.pageIndex(self.maxPageIndex());
            }
        };

    }

    var CardViewModel = function () {
        var self = this;
        self.cards = ko.observableArray([]);
        self.seriesList = ko.observableArray([]);

        // Load initial state from server, convert it to Item instances, then populate self.items
        function loadResults(allData) {
            var mappedItems = $.map(allData, function(item) { return new Card(item) });
            self.cards(mappedItems);
            // manage series in order to get only distincts edition
            var duplicatedSeries = $.map(allData, function(item) { return new Serie(item) });
            var uniqueSeriesObject = {}; // create object and use it as associative array
            var uniqueSeriesArray = []; // our unique series
            $.each(duplicatedSeries, function (index, value) {
                if (!uniqueSeriesObject[value.Series()]) {
                    uniqueSeriesObject[value.Series()] = value;
                    uniqueSeriesArray.push(value);
                }
            });
            uniqueSeriesArray.sort(function(a, b) {
                return (a.SeriesNM_FR().localeCompare( b.SeriesNM_FR()) < 0) ? -1 : 1;
            });
            self.seriesList(uniqueSeriesArray);
        }
        loadResults(JSON.parse(collection));

        // Manage details panel
        self.selectedCard = ko.observable();

        self.details = function (card) {
            self.selectedCard(card);
            // Change card image by remote file if file doesn't exist on server
            if ( ! card.LocalImage.fileExists() ) {
                document.getElementById('cardImage').setAttribute("src",card.RemoteImage);
            }
        };

        // Global stats
        self.nbCardsCollection = ko.computed(function () {
            return self.cards().length;
        }, self);

        self.nbCardsCollectionTotal = ko.computed(function () {
            var total = 0;
            for (var i = 0; i < self.cards().length; i++) {
                total += self.cards()[i].Items();
            }
            return total;
        }, self);

        self.collectionPrice = ko.computed(function () {
            var total = 0;
            for (var i = 0; i < self.cards().length; i++) {
                total += self.cards()[i].Items()*self.cards()[i].Price();
            }
            return total.toFixed(2);
        }, self);

        self.collectionRarity = ko.computed(function() {
            tabRarity = new Array();
            tabRarity["C"]=0;
            tabRarity["U"]=0;
            tabRarity["R"]=0;
            tabRarity["M"]=0;
            for (var i = 0; i < self.cards().length; i++) {
                if ( self.cards()[i].Rarity() == "C" )
                    tabRarity["C"] += self.cards()[i].Items();
                else if ( self.cards()[i].Rarity() == "U" )
                    tabRarity["U"] += self.cards()[i].Items();
                else if ( self.cards()[i].Rarity() == "R" )
                    tabRarity["R"] += self.cards()[i].Items();
                else if ( self.cards()[i].Rarity() == "M" )
                    tabRarity["M"] += self.cards()[i].Items();
                else
                    tabRarity["C"] += self.cards()[i].Items();
            }
            return tabRarity;
        }, self);

        self.collectionMythique = ko.computed(function () {
            return self.collectionRarity()["M"];
        }, self);

        self.collectionRare = ko.computed(function () {
            return self.collectionRarity()["R"];
        }, self);

        self.collectionUnco = ko.computed(function () {
            return self.collectionRarity()["U"];
        }, self);

        self.collectionCommon = ko.computed(function () {
            return self.collectionRarity()["C"];
        }, self);

        self.seriesListLength = ko.computed(function () {
            return self.seriesList().length;
        }, self);

        // filter cards by edition
        self.editionToShow = ko.observable();

        self.filteredList = ko.dependentObservable(function() {
            // Represents a filtered list of cards
            // i.e., only those matching the "editionToShow" condition
            var desiredType = self.editionToShow();

            if (desiredType == undefined) return self.cards();
                return ko.utils.arrayFilter(self.cards(), function(card) {
                    return card.Series() == desiredType.Series();
                });
        });

        self.nbCards = ko.computed(function () {
            return self.filteredList().length;
        }, self);

        self.nbCardsTotal = ko.computed(function () {
            var total = 0;
            for (var i = 0; i < self.filteredList().length; i++) {
                total += self.filteredList()[i].Items();
            }
            return total;
        }, self);

        self.EditionLogo = ko.computed(function() {
            if ( self.editionToShow() != undefined ) {
                return './img/logos/'+ self.editionToShow().Folder() + '.gif';
            }
            return '';
        }, self);

        self.EditionSymbole = ko.computed(function() {
            if ( self.editionToShow() != undefined ) {
                LocalEditionSymbole = './img/symboles/'+ self.editionToShow().Folder() + '.gif';
                RemoteEditionSymbole = 'http://mtgimage.com/symbol/set/' + self.editionToShow().Folder() + "/c/" + '24.gif';
                if ( LocalEditionSymbole.fileExists() ) {
                    return LocalEditionSymbole;
                }else{
                    return RemoteEditionSymbole;
                }
            }else{
                return '';
            }
        }, self);

        self.editionPrice = ko.computed(function () {
            var total = 0;
            for (var i = 0; i < self.filteredList().length; i++) {
                total += self.filteredList()[i].Items()*self.filteredList()[i].Price();
            }
            return total.toFixed(2);
        }, self);

        self.editionRarity = ko.computed(function() {
            tabEditionRarity = new Array();
            tabEditionRarity["C"]=0;
            tabEditionRarity["U"]=0;
            tabEditionRarity["R"]=0;
            tabEditionRarity["M"]=0;
            for (var i = 0; i < self.filteredList().length; i++) {
                if ( self.filteredList()[i].Rarity() == "C" )
                    tabEditionRarity["C"] += self.filteredList()[i].Items();
                else if ( self.filteredList()[i].Rarity() == "U" )
                    tabEditionRarity["U"] += self.filteredList()[i].Items();
                else if ( self.filteredList()[i].Rarity() == "R" )
                    tabEditionRarity["R"] += self.filteredList()[i].Items();
                else if ( self.filteredList()[i].Rarity() == "M" )
                    tabEditionRarity["M"] += self.filteredList()[i].Items();
                else
                    tabEditionRarity["C"] += self.filteredList()[i].Items();
            }
            return tabEditionRarity;
        }, self);

        self.editionMythique = ko.computed(function () {
            return self.editionRarity()["M"];
        }, self);

        self.editionRare = ko.computed(function () {
            return self.editionRarity()["R"];
        }, self);

        self.editionUnco = ko.computed(function () {
            return self.editionRarity()["U"];
        }, self);

        self.editionCommon = ko.computed(function () {
            return self.editionRarity()["C"];
        }, self);

        // search cards
        self.searchField = ko.observable();
        self.throttledSearchField = ko.computed(self.searchField).extend({ throttle: 400 });

        self.filteredItems = ko.computed(function() {
            var textFilter = self.throttledSearchField();
            if (!textFilter) {
                return self.filteredList();
            } else {
                var lowerCaseFilter = textFilter.toLowerCase();
                return ko.utils.arrayFilter(self.filteredList(), function(card) {
                    var pattern = new RegExp(lowerCaseFilter, 'i');
                    var titleResult =  pattern.test(card.Title());
                    var titleFRResult =   pattern.test(card.TitleFR());
                    var subTypeVFResult =  card.SubTypeVF() ?  pattern.test(card.SubTypeVF()): false;
                    return titleResult || titleFRResult || subTypeVFResult;
                });
            }
        }, self);

        // Filter and paginate by color
        self.white = ko.dependentObservable(function() {
            return new ColorList(ko.utils.arrayFilter(self.filteredItems(), function(card) {
                return card.Color() == "White";
            }));
        });

        self.blue = ko.dependentObservable(function() {
            return new ColorList(ko.utils.arrayFilter(self.filteredItems(), function(card) {
                return card.Color() == "Blue";
            }));
        });

        self.green = ko.dependentObservable(function() {
            return new ColorList(ko.utils.arrayFilter(self.filteredItems(), function(card) {
                return card.Color() == "Green";
            }));
        });

        self.red = ko.dependentObservable(function() {
            return new ColorList(ko.utils.arrayFilter(self.filteredItems(), function(card) {
                return card.Color() == "Red";
            }));
        });

        self.black = ko.dependentObservable(function() {
            return new ColorList(ko.utils.arrayFilter(self.filteredItems(), function(card) {
                return card.Color() == "Black";
            }));
        });

        self.multi = ko.dependentObservable(function() {
            return new ColorList(ko.utils.arrayFilter(self.filteredItems(), function(card) {
                return card.Color() == "Multicolor";
            }));
        });

        self.artifacts = ko.dependentObservable(function() {
            return new ColorList(ko.utils.arrayFilter(self.filteredItems(), function(card) {
                return card.Color() == "Colorless";
            }));
        });

        self.lands = ko.dependentObservable(function() {
            return new ColorList(ko.utils.arrayFilter(self.filteredItems(), function(card) {
                return card.Cost() == "";
            }));
        });
    };

    // Activates knockout.js
    var model = new CardViewModel();
    ko.applyBindings(model);

})();
