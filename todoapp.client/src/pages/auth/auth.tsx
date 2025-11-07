import React, { useEffect, useState } from 'react';
import { motion } from 'framer-motion';

import LoginForm from './loginForm';
import SignupForm from './signupForm';
import { removeToken, removeUser } from '../../utils/tokenManage';
import landing from '../../assets/png/landing.png';



const EnterAnimation = ({ ball }: { ball: React.CSSProperties }) => {
    return (
        <motion.div
            className="z-10"
            initial={{ scale: 1 }}
            animate={{
                scale: [1, 1.3, 1, 1.3, 1],
            }}

            transition={{
                duration: 5,
                ease: "easeInOut",
                repeat: Infinity,
            }}
            style={ball}
        />
    );
};

export const LogOut=()=>{
   removeUser();
   removeToken();
}

const AuthPage = () => {
    const ball = {
        width: 200,
        height: 200,
        backgroundColor: "#010438",
        borderRadius: "50%",
    };
    const square = {
        width: 200,
        height: 200,
        backgroundColor: "#010438",
    };

    const [isLogin, setIsLogin] = useState(true);
    const childCss = "flex-1 h-full rounded-xl bg-primary-50 flex items-center justify-center relative overflow-hidden";


    const slideDistance = window.innerWidth * 0.5;

    //console.log(slideDistance);

    return (
        <div className="min-h-screen  p-8 flex h-96 items-center gap-8 justify-center">
            <div
                className="flex w-full h-full gap-8"
            >
                <motion.div
                    layout
                    className={childCss}
                    initial={{ x: 0 }}
                    animate={{ x: isLogin ? slideDistance : 0, backgroundColor: isLogin ? "#ddc7a1" : "#7daea3" }}
                    transition={{ duration: 0.5 }}
                >
                    <motion.div
                        key={isLogin ? "login" : "signup"} 
                        initial={{ opacity: 0, x: 50 }}
                        animate={{ opacity: 1, x: 0 }}
                        exit={{ opacity: 0, x: -50 }}
                        transition={{ duration: 0.5, ease: "easeInOut" }}
                    >
                        {isLogin ? <LoginForm /> : <SignupForm setIsLogin={setIsLogin}/>}
                    </motion.div>
                    <button
                        onClick={() => setIsLogin(!isLogin)}
                        className="absolute bottom-4 text-sm text-blue-600 hover:text-blue-800"
                    >
                        {isLogin ? "Need an account? Sign up" : "Already have an account? Login"}
                    </button>
                </motion.div>

                <motion.div
                    layout
                    className={childCss}
                    initial={{ x: 0 }}
                    animate={{ x: isLogin ? -slideDistance : 0 }} // slide left if isLogin is true
                    transition={{ duration: 0.5 }}
                >
                    <EnterAnimation ball={ball} />
                    <img
                        src={landing}
                        alt="Beautiful landscape"
                        className="absolute inset-0 w-full h-full object-cover z-0"
                    />
                </motion.div>
            </div>
        </div>
    );
};

export default AuthPage;