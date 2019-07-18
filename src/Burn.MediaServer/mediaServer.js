"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const NodeMediaServer = require('node-media-server');
const config = {
    rtmp: {
        port: 1935,
        chunk_size: 128,
        gop_cache: true,
        ping: 30,
        ping_timeout: 60
    },
    http: {
        port: 8000,
        mediaroot: './media',
        webroot: './www',
        allow_origin: '*'
    },
    auth: {
        api: true,
        api_user: 'admin',
        api_pass: 'admin',
        play: false,
        publish: false,
        secret: 'nodemedia2017privatekey'
    },
};
let nms = new NodeMediaServer(config);
exports.StartMediaServer = () => {
    nms.run();
};
exports.default = nms;
//# sourceMappingURL=mediaServer.js.map