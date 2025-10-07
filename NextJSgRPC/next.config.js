/** @type {import('next').NextConfig} */
const nextConfig = {
  // Allow gRPC calls from browser (API routes)
  webpack: (config) => {
    config.externals = [...(config.externals || []), { '@grpc/grpc-js': '@grpc/grpc-js' }];
    return config;
  },
};

module.exports = nextConfig;
