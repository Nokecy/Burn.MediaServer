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

export const StartMediaServer = () => {
    nms.run();
}

export default nms;

