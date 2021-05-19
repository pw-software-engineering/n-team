class CarouselPictureManager {
    constructor(imgDisplayHandle) {
        this.imgDisplayHandle = imgDisplayHandle;
    }

    showNextPicture() {
        if (this._pictures.length === 0) {
            return;
        }
        this.showPictureIdx((this.currentPictureIdx + 1) % this._pictures.length);
    }

    showPreviousPicture() {
        if (this._pictures.length === 0) {
            return;
        }
        this.showPictureIdx((this._pictures.length + this.currentPictureIdx - 1) % this._pictures.length);
    }

    showPictureIdx(index) {
        if (index < 0 || index >= this._pictures.length) {
            throw new RangeError("index");
        }
        this.currentPictureIdx = index;
        this.imgDisplayHandle.attr("src", this._pictures[index]);
    }

    set pictures(pictures) {
        if (!pictures || !Array.isArray(pictures)) {
            pictures = [];
        }
        this._pictures = pictures;
        if (this._pictures.length === 0) {
            this.currentPictureIdx = -1;
            this.imgDisplayHandle.attr("src", "/resources/no-image-available.jpg");
            return;
        }
        this.showPictureIdx(0);
    }
}