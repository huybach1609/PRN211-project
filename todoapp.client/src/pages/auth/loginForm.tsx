import { useState } from "react";
import { Button } from "@heroui/button";
import { Input } from "@heroui/input";
import { addToast } from "@heroui/react";
import { useNavigate } from "react-router-dom";
import { AuthService, LoginRequest } from "../../services/authservice";
import { useAuth } from "../../contexts/AuthContext";
import { AuthResponse } from "../../types/User";
import axios from "axios";


interface LoginResponse {
   
}

const LoginForm = () => {
    const navigate = useNavigate();
    const { login } = useAuth();

    const [formData, setFormData] = useState<LoginRequest>({} as LoginRequest);


    const handleSubmit = async () => {
        if (!formData.username || !formData.password) {
            addToast({
                title: "Please fill in all fields",
                color: "danger",
            });
            return;
        }
        try {

            const response = await AuthService.login(formData);
            const loginResponse = response.data;
            console.log("Data: ",loginResponse);

            if (loginResponse.result) {
                addToast({
                    title: loginResponse.message,
                    promise: new Promise((resolve) => setTimeout(resolve, 3000)),
                });

                // proccess to save token and load user data
                login(loginResponse.accessToken);

                setFormData({} as LoginRequest);

                setTimeout(() => {
                    navigate("/");
                }, 2000);
            }
        } catch (error) {
            if (axios.isAxiosError(error) && error.response) {
                const errorMessage =
                    error.response.data?.message || "An error occurred during login";
                addToast({
                    title: errorMessage,
                    color: "danger",
                });
            } else {
                const errorMessage = error instanceof Error ? error.message : "An unexpected error occurred";
                addToast({
                    title: errorMessage,
                    color: "danger",
                });
            }
        }
    };

    return (
        <div className="w-auto">
            <h2 className="text-2xl font-bold mb-6 text-green">Login</h2>
            <form onSubmit={(e) => { e.preventDefault(); handleSubmit(); }} method="post">
                <div className="w-[50vh]">
                    <Input
                        label="Username"
                        type="text"
                        placeholder="username or email"
                        className="my-5"
                        required={true}
                        value={formData.username}
                        onChange={(e) => setFormData({ ...formData, username: e.target.value })}
                    />
                    <Input
                        label="Password"
                        placeholder="Enter your password"
                        type="password"
                        className="my-5"
                        required={true}
                        value={formData.password}
                        onChange={(e) => setFormData({ ...formData, password: e.target.value })}
                    />
                    <Button
                        type="submit"
                        size="sm"
                        variant="flat"
                        className="w-full bg-green"
                    >
                        Sign In
                    </Button>
                </div>
            </form>
        </div>
    );
};
export default LoginForm;