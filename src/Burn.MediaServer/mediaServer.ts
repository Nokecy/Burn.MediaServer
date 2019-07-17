const NodeMediaServer = require('node-media-server');

const config = {
    rtmp: {
        port: 1935,
        chunk_size: 128,
        gop_cache: true,
        ping: 10,
        ping_timeout: 3
    }
};

let nms = new NodeMediaServer(config);

export const StartMediaServer = () => {
    nms.run();
}

export default nms;

