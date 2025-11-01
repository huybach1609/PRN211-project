import { Button } from "@heroui/button";
import { Input } from "@heroui/input";
import { addToast } from "@heroui/react";
import axios from "axios";
import { API_URL } from "../../constrains";
import { user } from "@heroui/theme";
import { setToken, setUserLog } from "../../utils/tokenManage";
import { useNavigate } from "react-router-dom";


const LoginForm = () => {

    const processLogin = (username, password) => {
        //console.log(username, password);
        //console.log(API_URL);
        console.log(API_URL + '/auth/login');
        console.log(username, password);
        return axios
            .post(API_URL + '/auth/login',
                { username, password },
                {
                    headers: {
                        'Content-Type': 'application/json'
                    },
                });
    }

    const navigate = useNavigate();
    const LoginPerform = (e) => {
        e.preventDefault();

        var username = e.target.username.value;
        var password = e.target.password.value;
        //console.log(username, password);

        processLogin(username, password).then((response) => {
            //console.log(response.data);
            if (response.data.success) {
                //console.log(response.data.message);
                addToast({
                    title: response.data.message,
                    promise: new Promise((resolve) => setTimeout(resolve, 3000))
                });

                setUserLog(JSON.stringify(response.data.account));
                setToken(response.data.key);


                setTimeout(() => {
                    navigate("/");
                }, 2000);

            }
        }).catch((error) => {
            if (error.response) {
                // Handle known errors (like 400 Bad Request)
                //console.log('Error:', error.response.data.message);
                addToast({ title: error.response.data.message, color: "danger" });
            } else {
                // Handle network errors or unexpected issues
                console.error('Unexpected error:', error.message);
            }
        });
    }

    return (
        <div className="w-auto">
            <h2 className="text-2xl  font-bold mb-6 text-green">Login</h2>
            <form onSubmit={LoginPerform} method="post">
                <div className="w-[50vh]">
                    <Input label="Username" type="text" placeholder="username or email"
                        className="my-5" required={true} name="username" />
                    <Input label="Password" placeholder="Enter your password" type="password"
                        className="my-5" required={true} name="password" />
                    <Button type="submit"
                        size="sm"
                        variant="flat"
                        className="w-full bg-green"
                    >Sign In</Button>
                </div>
            </form>
        </div>
    );
}
export default LoginForm;