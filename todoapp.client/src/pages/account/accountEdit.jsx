import { useEffect, useState } from "react";
import { GetAccountInfor } from "./accountInfo";
import { Input, Spinner } from "@heroui/react";
import DefaultLayout from "../../layouts/default";


export const AccountEdit = () => {
    const [user, setUser] = useState(null);

    useEffect(() => {
        const fetchUser = async () => {
            try {
                const data = await GetAccountInfor();
                setUser(data);
            } catch (error) {
                console.error("Failed to load account info:", error);
            }
        };
        fetchUser();
    }, []);

    if (!user) {
        return <Spinner />; // Show loading indicator
    }

    return (
        <DefaultLayout>
            <div className="m-10 w-full">
                <div className="px-4 sm:px-0">
                    <h3 className="text-base/7 font-semibold ">Applicant Information</h3>
                    <p className="mt-1 max-w-2xl text-sm/6 text-green ">Personal details and application.</p>
                </div>
                <div className="mt-6 border-t border-green">
                    <dl className="divide-y divide-green">
                        <div className="px-4 py-6 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-0">
                            <dt className="text-sm/6 font-medium ">Full name</dt>
                            <dd className="mt-1 text-sm/6  sm:col-span-2 sm:mt-0">
                                <Input variant="underlined" value={user.fullName} />
                                {/* {user.fullName} */}
                            </dd>
                        </div>
                        <div className="px-4 py-6 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-0">
                            <dt className="text-sm/6 font-medium ">UserName</dt>
                            <dd className="mt-1 text-sm/6  sm:col-span-2 sm:mt-0">{user.userName}</dd>
                        </div>
                        <div className="px-4 py-6 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-0">
                            <dt className="text-sm/6 font-medium ">Email address</dt>
                            <dd className="mt-1 text-sm/6  sm:col-span-2 sm:mt-0">
                                <Input variant="underlined" value={user.email} />
                                {/* {user.email} */}
                            </dd>
                        </div>

                        <div className="px-4 py-6 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-0">
                            <dt className="text-sm/6 font-medium ">Attachments</dt>
                            <dd className="mt-2 text-sm  sm:col-span-2 sm:mt-0">
                                <ul role="list" className="divide-y divide-green rounded-md border border-green">
                                    <li className="flex items-center justify-between py-4 pl-4 pr-5 text-sm/6">
                                        <div className="flex w-0 flex-1 items-center">
                                            {/* <PaperClipIcon aria-hidden="true" className="size-5 shrink-0 text-gray-400" /> */}
                                            <div className="ml-4 flex min-w-0 flex-1 gap-2">
                                                <span className="truncate font-medium">resume_back_end_developer.pdf</span>
                                                <span className="shrink-0 text-green">2.4mb</span>
                                            </div>
                                        </div>
                                        <div className="ml-4 shrink-0">
                                            <a href="#" className="font-medium  text-green hover:text-green/80">
                                                Download
                                            </a>
                                        </div>
                                    </li>
                                    <li className="flex items-center justify-between py-4 pl-4 pr-5 text-sm/6">
                                        <div className="flex w-0 flex-1 items-center">
                                            {/* <PaperClipIcon aria-hidden="true" className="size-5 shrink-0 text-gray-400" /> */}
                                            <div className="ml-4 flex min-w-0 flex-1 gap-2">
                                                <span className="truncate font-medium">coverletter_back_end_developer.pdf</span>
                                                <span className="shrink-0 text-green">4.5mb</span>
                                            </div>
                                        </div>
                                        <div className="ml-4 shrink-0">
                                            <a href="#" className="font-medium text-green hover:text-green/80">
                                                Download
                                            </a>
                                        </div>
                                    </li>
                                </ul>
                            </dd>
                        </div>
                    </dl>
                </div>
            </div>
        </DefaultLayout>
    )
}
