function Pager(onPageChange) {
    this.pageNumber = -1;
    this.pageSize = 10;
    this.pageNumberInputs = [];
    this.onPageChange = onPageChange;
}

Pager.prototype.setPageNumber = function (number, forceChange = false) {
    number = Math.floor(number);
    if (number <= 0) {
        number = 1;
    }
    var loadNewPage = this.pageNumber !== number;
    this.pageNumber = number;
    for (var i = 0; i < this.pageNumberInputs.length; i++) {
        this.pageNumberInputs[i].val(this.pageNumber);
    }
    if (!loadNewPage && !forceChange) {
        return;
    }
    this.onPageChange(this.pageNumber, this.pageSize);
}

Pager.prototype.getPageNumber = function () {
    return this.pageNumber;
}

Pager.prototype.setPageSize = function (pageSize) {
    this.pageSize = pageSize;
    this.onPageChange(this.pageNumber, this.pageSize);
}

Pager.prototype.getPageSize = function () {
    return this.pageSize;
}

Pager.prototype.addPageNumberInput = function (input) {
    this.pageNumberInputs.push(input);
}