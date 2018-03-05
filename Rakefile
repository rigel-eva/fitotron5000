require "JSON"
desc "Building Our Docker Image"
task :default do
    sh "cd Fitotron5000; docker build . -t fitotron5000:0.0.1"
end
desc "Quick building an instance of our docker image"
task :runInteractive do
    baseCommand="docker run --rm -it"
    keys=JSON::parse(File.read("./keys.json"))
    keys.each{|keyName,keyValue|
        baseCommand+=" -e \"#{keyName}=#{keyValue}\""
    }
    baseCommand+= " fitotron5000:0.0.1"
    sh baseCommand
end