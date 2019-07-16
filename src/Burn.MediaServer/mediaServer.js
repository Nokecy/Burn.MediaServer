"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const NodeMediaServer = require('node-media-server');
const config = {
    rtmp: {
        port: 1935,
        chunk_size: 128,
        gop_cache: true,
        ping: 60,
        ping_timeout: 30
    }
};
let nms = new NodeMediaServer(config);
exports.StartMediaServer = () => {
    nms.run();
};
exports.default = nms;
//# sourceMappingURL=mediaServer.js.map