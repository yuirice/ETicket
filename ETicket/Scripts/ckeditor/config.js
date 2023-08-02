/**
 * @license Copyright (c) 2003-2022, CKSource Holding sp. z o.o. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function( config ) {
    config.language = 'zh-TW';
    config.uiColor = '#AADC6E';
    config.basicEntities = false;
    config.entities = false;
    config.height = 500;
    //�Ϥ��q�{��ܤ�r����
    config.image_previewText = ' ';
    // �ѨMCKEditor�Ϥ��e�צ۾A�������D p img {    width: auto;    height: auto;    max - width: 100 %;}
    config.disallowedContent = 'img{ width, height }; img[width, height]';
};
