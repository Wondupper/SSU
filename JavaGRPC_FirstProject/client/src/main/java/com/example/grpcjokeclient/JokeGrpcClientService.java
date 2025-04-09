package com.example.grpcjokeclient;

import com.example.grpcjokeserver.grpc.JokeServiceGrpc;
import com.example.grpcjokeserver.grpc.JokeServiceProto.AgeRequest;
import com.example.grpcjokeserver.grpc.JokeServiceProto.Empty;
import com.example.grpcjokeserver.grpc.JokeServiceProto.GenreRequest;
import com.example.grpcjokeserver.grpc.JokeServiceProto.JokeResponse;
import net.devh.boot.grpc.client.inject.GrpcClient;
import org.springframework.stereotype.Service;

@Service
public class JokeGrpcClientService {

    @GrpcClient("jokeServer")
    private JokeServiceGrpc.JokeServiceBlockingStub jokeServiceBlockingStub;

    public String getRandomJoke() {
        Empty request = Empty.newBuilder().build();
        JokeResponse response = jokeServiceBlockingStub.getRandomJoke(request);
        return response.getJoke();
    }

    public String getJokeByGenre(String genre) {
        GenreRequest request = GenreRequest.newBuilder()
                .setGenre(genre)
                .build();
        JokeResponse response = jokeServiceBlockingStub.getJokeByGenre(request);
        return response.getJoke();
    }

    public String getJokeByAge(int age) {
        AgeRequest request = AgeRequest.newBuilder()
                .setAge(age)
                .build();
        JokeResponse response = jokeServiceBlockingStub.getJokeByAge(request);
        return response.getJoke();
    }
}
